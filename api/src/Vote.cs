using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Api.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
using Azure.Data.Tables;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace Question
{
    //Votes and Gets the Current Votes
    public static class Vote
    {
        [FunctionName("Vote")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "polls/{id}/question")] HttpRequest req,
            [SignalR(HubName = "poll", ConnectionStringSetting = "AzureSignalRConnectionString")] IAsyncCollector<SignalRMessage> signalRMessages,
            string id)
        {
            //Fetch Answer Id from Body
            string requestBody = string.Empty;
            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            int submittedAnswerId = data?.answerId;

            //Connect to table storage
            var storageUri = new Uri(Environment.GetEnvironmentVariable("StorageUri"));
            var tableClient = new TableClient(storageUri, "Votes", new Azure.Identity.DefaultAzureCredential());

            //Write new vote
            var tableEntity = new TableEntity(id, Guid.NewGuid().ToString())
            {
                {"OptionId", submittedAnswerId }
            };
            tableClient.AddEntity(tableEntity);

            //Get Latest Results
            var votes = tableClient.Query<VoteDataRow>(ent => ent.PartitionKey.Equals(id, StringComparison.Ordinal));
             
            var total = votes.Count();
            var response = votes.GroupBy(a => a.OptionId)
                .Select(group => new PollResult()
                {
                    AnswerId = group.Key,
                    Percentage = (int)Math.Round((double)(100 * group.Count()) / total)
                });

            //notify any other subscribers
            await signalRMessages.AddAsync(new SignalRMessage
            {
                GroupName = id,
                Target = "new-results",
                Arguments = new object[] { response.ToArray() }
            });

            return new OkObjectResult(response);
        }
    }
}

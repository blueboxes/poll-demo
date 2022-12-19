using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Api.Models;
using Azure.Data.Tables;
using System.Linq;

namespace Api
{
    /// <summary>
    /// Gets the Question and the Options
    /// </summary>
    public static class QuestionPrompt
    {
        [FunctionName("GetQuestionAndOptions")]
        public static IActionResult GetQuestionAndOptions(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "polls/{id}/question")] HttpRequest req,
            string id)
        {            
            var storageUri = new Uri(Environment.GetEnvironmentVariable("StorageUri"));
            var tableClient = new TableClient(storageUri, "Questions", new Azure.Identity.DefaultAzureCredential());
            var questionOptions = tableClient.Query<QuestionDataRow>(ent => ent.PartitionKey.Equals(id, StringComparison.Ordinal));

            if (questionOptions.Count() == 0)
                return new NotFoundObjectResult(null);

            var response = new PollQuestion
            {
                QuestionId = id,
                QuestionText = questionOptions.First(a => !string.IsNullOrWhiteSpace(a.Question)).Question
            };

            foreach (var option in questionOptions)
            {
                response.Options.Add(new PollOption() { AnswerId = int.Parse(option.RowKey), Prompt = option.OptionText });
            }
          
            return new OkObjectResult(response);
        }
    }
}

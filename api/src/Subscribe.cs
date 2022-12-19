using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Api.Models;

namespace Question
{
    public static class Subscribe
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "poll",
            ConnectionStringSetting = "AzureSignalRConnectionString")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }


        [FunctionName("Subscribe")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "polls/{id}/subscribe")] 
            Connection connection,
            [SignalR(HubName = "poll", ConnectionStringSetting = "AzureSignalRConnectionString")] 
            IAsyncCollector<SignalRGroupAction> signalRGroupActions)
        {
            await signalRGroupActions.AddAsync(
             new SignalRGroupAction
             {
                 ConnectionId = connection.ConnectionId,
                 GroupName = connection.Id,
                 Action = GroupAction.Add
             });

            return new OkResult();
        }
    }
}

using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MarcusTurewicz.Status
{
    public static class BroardcastStatus
    {
        [FunctionName("BroardcastStatus")]
        public static async Task Run(
            [BlobTrigger("container/{name}", Connection = "AzureWebJobsStorage")]Stream blobStream,
            [SignalR(HubName = "status")]IAsyncCollector<SignalRMessage> signalRMessages,
             string name, ILogger log)
        {
            using (var reader = new StreamReader(blobStream))
            {
                blobStream.Seek(0, SeekOrigin.Begin);
                var json = await reader.ReadToEndAsync();
                var status = JsonConvert.DeserializeObject<StatusEntity>(json);
                await signalRMessages.AddAsync(
                    new SignalRMessage
                    {
                        Target = "broadcast",
                        Arguments = new [] {status}
                    });
            }
        }
    }
}

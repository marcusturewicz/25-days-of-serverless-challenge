using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Linq;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slack.Webhooks;

namespace MarcusTurewicz.AnalyseGift
{
    public class AnalyseGift
    {
        [FunctionName("AnalyseGift")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {
            using (var reader = new StreamReader(req.Body))
            {
                var base64 = await reader.ReadToEndAsync();
                var bytes = Convert.FromBase64String(base64);

                using (var memStream = new MemoryStream(bytes))
                {

                    string key = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
                    string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

                    var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };

                    var features = new List<VisualFeatureTypes>()
                {
                    VisualFeatureTypes.Tags
                };

                    var result = await client.AnalyzeImageInStreamAsync(memStream, features);

                    var valid = new[] { "box", "gift wrapping", "ribbon", "present" };
                    bool isValid = !valid.Except(result.Tags.Select(x => x.Name)).Any();

                    if (isValid)
                    {
                        var slackMessage = new SlackMessage
                        {
                            Text = "Valid gift seen!"
                        };
                        var slackClient = new SlackClient(Environment.GetEnvironmentVariable("SlackWebHook"));                        
                        slackClient.Post(slackMessage); 
                    }
                }
            }
        }
    }
}

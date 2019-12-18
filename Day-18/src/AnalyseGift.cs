using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Linq;

namespace MarcusTurewicz.AnalyseGift
{
    public class AnalyseGift
    {
        [FunctionName("AnalyseGift")]
        public async Task Run([BlobTrigger("gifts/{name}", Connection = "AzureWebJobsStorage")]Stream blob, string name, ILogger log)
        {
            var extension = Path.GetExtension(name).ToLower();
            if (!extension.Equals(".png") && !extension.Equals(".jpg") && !extension.Equals(".jpeg"))
            {
                Console.WriteLine($"File with extension {extension} is not valid.");
                return;
            }

            string key = Environment.GetEnvironmentVariable("COMPUTER_VISION_SUBSCRIPTION_KEY");
            string endpoint = Environment.GetEnvironmentVariable("COMPUTER_VISION_ENDPOINT");

            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };

            var features = new List<VisualFeatureTypes>()
            {
                VisualFeatureTypes.Tags
            };

            var result = await client.AnalyzeImageInStreamAsync(blob, features);

            var valid = new[] { "box", "gift wrapping", "ribbon", "present" };
            bool isValid = !valid.Except(result.Tags.Select(x=>x.Name)).Any();

            if (!isValid)
            {
                Console.WriteLine("Present not wrapped correctly!!!");
                return;
            }

            Console.WriteLine("Present wrapped correclty!!!");
        }
    }
}

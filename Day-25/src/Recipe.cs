using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MarcusTurewicz.Recipe
{
    public static class Recipe
    {
        [FunctionName("recipe")]
        public static async Task Run(
            [BlobTrigger("audio/{name}", Connection = "AzureWebJobsStorage")] Stream inputStream,
            [Blob("recipe/{name}.txt", FileAccess.Write)] TextWriter outputWriter,
            string name, ExecutionContext context, ILogger log)
        {
            var key = Environment.GetEnvironmentVariable("SpeechSubscriptionKey");
            var region = Environment.GetEnvironmentVariable("SpeechRegion");
            var config = SpeechConfig.FromSubscription(key, region);
            var tempPath = Path.Combine(context.FunctionDirectory, Guid.NewGuid().ToString());
            using var outputFileStream = new FileStream(tempPath, FileMode.Create);
            await inputStream.CopyToAsync(outputFileStream);
            using var audioInput = AudioConfig.FromWavFileInput(tempPath);
            using var recognizer = new SpeechRecognizer(config, audioInput);
            var result = await recognizer.RecognizeOnceAsync();
            await outputWriter.WriteAsync(result.Text);
        }
    }
}

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace MarcusTurewicz.ImageSearch
{
    public static class ImageSearch
    {
        [FunctionName("ImageSearch")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Get search query
            string search = req.Query["s"];

            // Create image service
            var imageService = new ImageService(Environment.GetEnvironmentVariable("UNSPLASH_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("UNSPLASH_SECRET_KEY"));
            
            // Get image
            var image = await imageService.GetImageAsync(search);

            // Return as content
            var file = new FileContentResult(image.Bytes, "application/octet-stream");
            file.FileDownloadName = image.Filename;
            return file;

        }
    }
}

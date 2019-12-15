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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ImageSearch/{search}")] HttpRequest req, string search,
            ILogger log, ExecutionContext context)
        {
            // Create image service
            var imageService = new ImageService(Environment.GetEnvironmentVariable("UNSPLASH_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("UNSPLASH_SECRET_KEY"));
            
            // Get image
            var image = await imageService.GetImageAsync(search);

            // Return as content
            var file = new FileContentResult(image.Bytes, "image/png");
            file.FileDownloadName = image.Filename;
            return file;

        }
    }
}

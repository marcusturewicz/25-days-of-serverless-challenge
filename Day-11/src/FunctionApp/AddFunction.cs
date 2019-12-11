using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarcusTurewicz.WishSaver
{
    public static class AddFunction
    {
        [FunctionName("wishes")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "WishDB", collectionName: "Wishes", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<dynamic> document,
                       ILogger log)
        {
            var form = await req.ReadFormAsync();

            var wish = new Wish {
                Address = form["addr"],
                Description = form["desc"],
                Who = form["who"],
                Type = form["type"]
            };

            await document.AddAsync(wish);
        }
    }
}

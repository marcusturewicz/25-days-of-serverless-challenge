using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using System.Linq;

namespace MarcusTurewicz.WishSaver
{
    public static class QueryFunction
    {
        [FunctionName("query")]
        public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        [CosmosDB( ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
        ILogger log)
        {
            var wishesCollectionUri = UriFactory.CreateDocumentCollectionUri(databaseId: "WishDB", collectionId: "Wishes");
            var options = new FeedOptions { EnableCrossPartitionQuery = true };
            var wishes = client.CreateDocumentQuery<Wish>(wishesCollectionUri, options).AsEnumerable().ToArray();
            return new OkObjectResult(wishes);    
        }
    }
}

using Microsoft.Azure.WebJobs;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using System;

namespace MarcusTurewicz.WishSaver
{
    public static class NotifyFunction
    {
        [FunctionName("notify")]
        public static void Run([CosmosDBTrigger(
            databaseName: "WishDB",
            collectionName: "Wishes",
            ConnectionStringSetting = "CosmosDBConnection",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents,
            ILogger logger)
        {
            var slackClient = new SlackClient(Environment.GetEnvironmentVariable("SlackWebHook"));
            foreach (var doc in documents)
            {
                var slackMessage = new SlackMessage
                {
                    Text = documents[0].GetPropertyValue<string>("desc")
                };
                slackClient.Post(slackMessage);
            }
        }
    }
}

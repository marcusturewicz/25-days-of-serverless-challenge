using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using System.Text.Json;
using System.IO;
using Tweetinvi;

namespace MarcusTurewicz.Gävlebocken
{
    public static class Challenge
    {
        [FunctionName("webhook")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var consumerSecret = (await client.GetSecretAsync("TWITTERCONSUMERSECRET")).Value.Value;

            // Challenge
            if (req.Query.TryGetValue("crc_token", out var crcToken))
            {
                var crcTokenBytes = Encoding.UTF8.GetBytes(crcToken);
                using var hmac256 = new HMACSHA256(Encoding.UTF8.GetBytes(consumerSecret));
                var hash = hmac256.ComputeHash(crcTokenBytes);
                var base64Hash = Convert.ToBase64String(hash);

                return new OkObjectResult(new { response_token = $"sha256={base64Hash}" });
            }

            // Surface tweet

            var consumerKey = (await client.GetSecretAsync("TWITTERCONSUMERKEY")).Value.Value;            
            var accessToken = (await client.GetSecretAsync("TWITTERACCESSTOKEN")).Value.Value;            
            var secretToken = (await client.GetSecretAsync("TWITTERSECRETTOKEN")).Value.Value;            

            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, secretToken);

            var tweetEvent = await System.Text.Json.JsonSerializer.DeserializeAsync<TweetEvent>(req.Body);

            foreach (var tweet in tweetEvent.TweetCreateEvents)
            {
                if (tweet.Text.ToLowerInvariant().Contains("gävlebocken"))
                    Tweet.PublishRetweet(tweet.Id);
            }

            return new OkResult();
        }
    }
}

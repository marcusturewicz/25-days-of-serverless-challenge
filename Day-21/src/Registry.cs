using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Linq;

namespace MarcusTurewicz.GiftRegistry
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Registry
    {
        [JsonProperty("entries")]
        public List<string> Entries { get; set; }

        [JsonProperty("open")]
        public bool Open { get; set; }

        public void Create()
        {
            Open = true;
            Entries = new List<string>();
        }

        public void Add(string entry)
        {
            if (Open)
                Entries.Add(entry);
        }

        public void Finish() => Open = false;

        [FunctionName(nameof(Registry))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<Registry>();

        [FunctionName("open")]
        public static async Task<IActionResult> OpenRegistry(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registry")] HttpRequest req,
        [DurableClient] IDurableEntityClient client,
        [DurableClient] IDurableOrchestrationClient starter)
        {
            var id = Guid.NewGuid().ToString();
            var entityId = new EntityId(nameof(Registry), id);
            await client.SignalEntityAsync(entityId, "Create");
            await starter.StartNewAsync("starttimer", null, entityId);
            return new OkObjectResult(new { id = id });
        }

        [FunctionName("addentry")]
        public static async Task AddEntry(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "registry/{registryId}")] HttpRequest req,
        string registryId,
        [DurableClient] IDurableEntityClient client)
        {
            using (var reader = new StreamReader(req.Body))
            {
                var entry = JsonConvert.DeserializeObject<Entry>(await reader.ReadToEndAsync()).Value;
                var entityId = new EntityId(nameof(Registry), registryId);
                await client.SignalEntityAsync(entityId, "Add", entry);
            }
        }

        [FunctionName("closeregistry")]
        public static Task Close(
        [HttpTrigger(AuthorizationLevel.Anonymous, "lock", Route = "registry/{registryId}")] HttpRequest req,
        string registryId,
        [DurableClient] IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(Registry), registryId);
            return client.SignalEntityAsync(entityId, "Finish");
        }

        [FunctionName("starttimer")]
        public static async Task Timer(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            [DurableClient] IDurableEntityClient client)
        {
            var deadline = context.CurrentUtcDateTime.AddMinutes(5);
            await context.CreateTimer(deadline, CancellationToken.None);
            await client.SignalEntityAsync(context.GetInput<EntityId>(), "Finish");
        }

        [FunctionName("aggregate")]
        public static async Task<IActionResult> Get(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "registry/agg")] HttpRequest req,
        [DurableClient] IDurableEntityClient client)
        {
            var result = await client.ListEntitiesAsync(new EntityQuery(), CancellationToken.None);
            int registries = 0;
            int entries = 0;
            foreach (var item in result.Entities)
            {
                var registry = (await client.ReadEntityStateAsync<Registry>(item.EntityId)).EntityState;
                registries++;
                entries += registry.Entries != null ? registry.Entries.Count() : 0;
            }
            return new OkObjectResult(new { registries = registries, entries = entries });
        }

        [FunctionName("peakregistry")]
        public static async Task<IActionResult> Peak(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "registry/{registryId}")] HttpRequest req,
        string registryId,
        [DurableClient] IDurableEntityClient client)
        {
            var registry = (await client.ReadEntityStateAsync<Registry>(new EntityId(nameof(Registry), registryId))).EntityState;
            return new OkObjectResult(registry);
        }        
    }
}
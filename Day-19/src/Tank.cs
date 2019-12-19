using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace MarcusTurewicz.Balloons
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        [JsonProperty("filling")]
        public bool Filling { get; set; }

        [JsonProperty("lastFillTime")]
        public DateTime LastFillTime { get; set; }

        [JsonProperty("ballons")]
        public int Ballons { get; set; }

        public void Start()
        {
            LastFillTime = DateTime.UtcNow;
            Filling = true;
        } 

        public void Stop()
        {
            var now = DateTime.UtcNow;
            Filling = false;
            var fillDuration = now.Subtract(LastFillTime);
            LastFillTime = default;
            UpdateBalloonCount(fillDuration);
        }

        public void Empty()
        {
            Ballons = 0;
        }

        public void Update()
        {
            // If tank on, need to update state
            var now = DateTime.UtcNow;
            if (Filling)
            {
                var fillDuration = now.Subtract(LastFillTime);
                LastFillTime = now;
                UpdateBalloonCount(fillDuration);
            }
        }

        private void UpdateBalloonCount(TimeSpan fillDuration)
        {
            // Max tank pressure is 200 Bar
            // Tank filled at 25 Bar/min
            // 1 balloon used 0.6 Bar
            var maxBalloons = (int)Math.Round(200 / 0.6);
            var balloonsFilled = (int)Math.Round(25 * fillDuration.TotalMinutes / 0.6);
            Ballons = balloonsFilled + Ballons > maxBalloons ? maxBalloons : balloonsFilled + Ballons;
        }

        [FunctionName(nameof(Tank))]
        public static Task Dispatch([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<Tank>();

        // This would be triggered by Azure IOT Hub, simulated by http trigger for now
        [FunctionName("operatetank")]
        public static Task Operate(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "operatetank/{operation}")] HttpRequest req, 
        string operation,
        [DurableClient] IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(Tank), "tank");
            return client.SignalEntityAsync(entityId, operation);
        }

        [FunctionName("querytank")]
        public static async Task<IActionResult> Query(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(Tank), "tank");
            await client.SignalEntityAsync(entityId, "Update");
            var state = await client.ReadEntityStateAsync<Tank>(entityId);
            return new OkObjectResult(state);
        }        
    }
}
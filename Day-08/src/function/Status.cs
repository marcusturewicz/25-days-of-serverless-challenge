using System;
using Newtonsoft.Json;

namespace MarcusTurewicz.Status
{
    public class StatusEntity
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
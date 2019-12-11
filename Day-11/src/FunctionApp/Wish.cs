using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MarcusTurewicz.WishSaver
{
    public class Wish
    {
        [JsonProperty("desc")]
        public string Description { get; set; }
        [JsonProperty("who")]
        public string Who { get; set; }
        [JsonProperty("addr")]
        public string Address { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
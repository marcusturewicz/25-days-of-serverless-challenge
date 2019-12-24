using Newtonsoft.Json;

namespace MarcusTurewicz.GiftRegistry
{
    public class Entry
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
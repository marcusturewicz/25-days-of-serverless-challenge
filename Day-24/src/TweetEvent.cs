// Generated by https://quicktype.io

using System.Text.Json.Serialization;

namespace MarcusTurewicz.Gävlebocken
{

    public partial class TweetEvent
    {
        [JsonPropertyName("tweet_create_events")]
        public TweetObject[] TweetCreateEvents { get; set; }
    }
}

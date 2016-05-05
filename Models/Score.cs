using Newtonsoft.Json;

namespace LanfeustBridge.Models
{
    public class Score
    {
        public int DealId { get; set; }
        public string Vulnerability { get; set; }
        public int Round { get; set; }
        public bool Entered { get; set; }
        public Players Players { get; set; }
        public Contract Contract { get; set; }
        public int Tricks { get; set; }
        [JsonProperty(PropertyName = "score")]
        public int BridgeScore { get; set; }
        [JsonProperty(PropertyName = "nsResult")]
        public double NSResult { get; set; }
        [JsonProperty(PropertyName = "ewResult")]
        public double EWResult { get; set; }
    }
}
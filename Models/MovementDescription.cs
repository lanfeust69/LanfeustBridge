using Newtonsoft.Json;

namespace LanfeustBridge.Models
{
    public class MovementDescription
    {
        /// <summary>
        /// The relative order in which we want to sort different movements for display
        /// </summary>
        [JsonIgnore]
        public int Order { get; set; } = 100;
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinTables { get; set; } = 1;
        public int MaxTables { get; set; } = -1;
        public int MinRounds { get; set; } = 1;
        public int MaxRounds { get; set; } = -1;
    }
}
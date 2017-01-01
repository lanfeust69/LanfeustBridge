namespace LanfeustBridge.Models
{
    public class MovementDescription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinTables { get; set; } = -1;
        public int MaxTables { get; set; } = -1;
        public int MinRounds { get; set; } = -1;
        public int MaxRounds { get; set; } = -1;
    }
}

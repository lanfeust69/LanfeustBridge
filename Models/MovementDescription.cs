namespace LanfeustBridge.Models;

public class MovementDescription
{
    /// <summary>
    /// The relative order in which we want to sort different movements for display.
    /// </summary>
    [JsonIgnore]
    public int Order { get; set; } = 100;

    public string Id { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int MinTables { get; set; } = 1;

    public int MaxTables { get; set; } = -1;

    public int MinRounds { get; set; } = 1;

    public int MaxRounds { get; set; } = -1;

    /// <summary>
    /// Used for movements with a fixed number of players.
    /// If not -1 (the default), takes precedence over number of tables.
    /// </summary>
    public int NbPlayers { get; set; } = -1;
}

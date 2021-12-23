namespace LanfeustBridge.Models;

public class Hands
{
    public Hands()
    {
        West = new Hand();
        North = new Hand();
        East = new Hand();
        South = new Hand();
    }

    public Hand West { get; set; }

    public Hand North { get; set; }

    public Hand East { get; set; }

    public Hand South { get; set; }
}

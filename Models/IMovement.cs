namespace LanfeustBridge.Models
{
    public interface IMovement
    {
        MovementDescription MovementDescription { get; }

        Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound);

        Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound);

        MovementValidation Validate(int nbTables, int nbRounds);
    }
}

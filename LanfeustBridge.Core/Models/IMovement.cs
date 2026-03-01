namespace LanfeustBridge.Models;

public interface IMovement
{
    MovementDescription MovementDescription { get; }

    /// <summary>
    /// Gets the list of positions of a tournament with the specified parameters.
    /// First indexed by round, then by player.
    /// </summary>
    Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound);

    Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound, int nbBoards);

    MovementValidation Validate(int nbTables, int nbRounds);
}

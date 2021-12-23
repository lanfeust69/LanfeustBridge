namespace LanfeustBridge.Tests;

public class TeamsTest
{
    private readonly Teams _teams = new Teams();

    public static IEnumerable<object[]> TeamsPositions
    {
        get
        {
            // first round
            yield return new object[] { 0, 0, new Position { Table = 0, Deals = new[] { 1, 2, 3, 4 }, North = 0, South = 1, East = 2, West = 3 } };
            yield return new object[] { 0, 6, new Position { Table = 1, Deals = new[] { 3, 4, 1, 2 }, North = 4, South = 5, East = 6, West = 7 } };
            // second round : switch players
            yield return new object[] { 1, 0, new Position { Table = 0, Deals = new[] { 5, 6, 7, 8 }, North = 0, South = 1, East = 4, West = 5 } };
            yield return new object[] { 1, 6, new Position { Table = 1, Deals = new[] { 7, 8, 5, 6 }, North = 2, South = 3, East = 6, West = 7 } };
        }
    }

    [Theory]
    [MemberData(nameof(TeamsPositions))]
    public void PositionsAreCorrect(int round, int player, Position expected)
    {
        var positions = _teams.GetPositions(2, 2, 4);
        var actual = positions[round][player];
        Assert.Equal(expected, actual, new PositionComparer());
    }

    [Theory]
    [InlineData(1, 1, true, 0)]
    [InlineData(3, 1, true, 0)]
    [InlineData(4, 1, true, 0)]
    [InlineData(2, 1, false, 6)]
    [InlineData(2, 4, false, 24)]
    public void DealsAreCorrect(int nbTables, int nbRounds, bool expectThrows, int expectedNumberOfDeals)
    {
        if (expectThrows)
        {
            Assert.ThrowsAny<Exception>(() => _teams.CreateDeals(nbTables, nbRounds, 6, 16));
            return;
        }
        var deals = _teams.CreateDeals(nbTables, nbRounds, 6, 16);
        Assert.Equal(expectedNumberOfDeals, deals.Length);
        // all deals played exactly twice
        foreach (var deal in deals)
            Assert.Equal(2, deal.Scores.Length);
    }
}

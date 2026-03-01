namespace LanfeustBridge.Tests;

public class Individual12Test
{
    private readonly Individual12 _individual = new Individual12();

    public static IEnumerable<object[]> IndividualPositions
    {
        get
        {
            // first serie of 3 deals
            yield return new object[] { 0, 0, new Position { Table = 0, Deals = new[] { 1, 2, 3 }, North = 11, South = 0, East = 5, West = 7 } };
            yield return new object[] { 0, 1, new Position { Table = 1, Deals = new[] { 2, 3, 1 }, North = 1, South = 8, East = 9, West = 6 } };
            yield return new object[] { 0, 2, new Position { Table = 2, Deals = new[] { 3, 1, 2 }, North = 3, South = 2, East = 4, West = 10 } };
            // second serie of 3 deals
            yield return new object[] { 1, 1, new Position { Table = 0, Deals = new[] { 4, 5, 6 }, North = 11, South = 1, East = 6, West = 8 } };
            yield return new object[] { 1, 2, new Position { Table = 1, Deals = new[] { 5, 6, 4 }, North = 2, South = 9, East = 10, West = 7 } };
            yield return new object[] { 1, 3, new Position { Table = 2, Deals = new[] { 6, 4, 5 }, North = 4, South = 3, East = 5, West = 0 } };
            // last serie of 3 deals
            yield return new object[] { 10, 4, new Position { Table = 0, Deals = new[] { 31, 32, 33 }, North = 11, South = 10, East = 4, West = 6 } };
            yield return new object[] { 10, 0, new Position { Table = 1, Deals = new[] { 32, 33, 31 }, North = 0, South = 7, East = 8, West = 5 } };
            yield return new object[] { 10, 1, new Position { Table = 2, Deals = new[] { 33, 31, 32 }, North = 2, South = 1, East = 3, West = 9 } };
        }
    }

    [Theory]
    [MemberData(nameof(IndividualPositions))]
    public void PositionsAreCorrect(int round, int player, Position expected)
    {
        var positions = _individual.GetPositions(3, 11, 3);
        var actual = positions[round][player];
        Assert.Equal(expected, actual, new PositionComparer());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(7, 8)]
    [InlineData(8, 1)]
    [InlineData(9, 2)]
    [InlineData(31, 8)]
    public void DealsAreCorrect(int index, int expectedEquivalent)
    {
        var deals = _individual.CreateDeals(3, 11, 3, 8);
        var deal = deals[index];
        Assert.Equal(index + 1, deal.Id);
        Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
        Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
    }

    [Fact]
    public void DealsScoresAreCorrect()
    {
        var deals = _individual.CreateDeals(2, 11, 3, 8);
        Assert.Equal(33, deals.Length);
        // all deals played exactly thrice, in the same round
        for (int i = 0; i < deals.Length; i++)
        {
            Assert.Equal(3, deals[i].Scores.Length);
            Assert.Equal(i / 3, deals[i].Scores[0].Round);
            Assert.Equal(i / 3, deals[i].Scores[1].Round);
            Assert.Equal(i / 3, deals[i].Scores[2].Round);
            Assert.Equal(0, deals[i].Scores[0].Table);
            Assert.Equal(1, deals[i].Scores[1].Table);
            Assert.Equal(2, deals[i].Scores[2].Table);
        }
    }
}

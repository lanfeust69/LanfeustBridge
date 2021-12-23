namespace LanfeustBridge.Tests;

public class TriplicateTest
{
    private readonly Triplicate _triplicate = new Triplicate();

    public static IEnumerable<object[]> TriplicatePositions
    {
        get
        {
            // first serie of 3 deals
            yield return new object[] { 0, 0, new Position { Table = 0, Deals = new[] { 1, 2, 3 }, North = 0, South = 1, East = 10, West = 11 } };
            yield return new object[] { 0, 2, new Position { Table = 1, Deals = new[] { 2, 3, 1 }, North = 2, South = 3, East = 8, West = 9 } };
            yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 3, 1, 2 }, North = 4, South = 5, East = 6, West = 7 } };
            // second serie of 3 deals
            yield return new object[] { 1, 0, new Position { Table = 0, Deals = new[] { 4, 5, 6 }, North = 0, South = 1, East = 8, West = 9 } };
            yield return new object[] { 1, 10, new Position { Table = 1, Deals = new[] { 5, 6, 4 }, North = 10, South = 11, East = 6, West = 7 } };
            yield return new object[] { 1, 2, new Position { Table = 2, Deals = new[] { 6, 4, 5 }, North = 2, South = 3, East = 4, West = 5 } };
            // third serie of 3 deals
            yield return new object[] { 2, 0, new Position { Table = 0, Deals = new[] { 7, 8, 9 }, North = 0, South = 1, East = 6, West = 7 } };
            yield return new object[] { 2, 8, new Position { Table = 1, Deals = new[] { 8, 9, 7 }, North = 8, South = 9, East = 4, West = 5 } };
            yield return new object[] { 2, 10, new Position { Table = 2, Deals = new[] { 9, 7, 8 }, North = 10, South = 11, East = 2, West = 3 } };
            // fourth serie of 3 deals
            yield return new object[] { 3, 0, new Position { Table = 0, Deals = new[] { 10, 11, 12 }, North = 0, South = 1, East = 4, West = 5 } };
            yield return new object[] { 3, 6, new Position { Table = 1, Deals = new[] { 11, 12, 10 }, North = 6, South = 7, East = 2, West = 3 } };
            yield return new object[] { 3, 8, new Position { Table = 2, Deals = new[] { 12, 10, 11 }, North = 8, South = 9, East = 10, West = 11 } };
            // last serie of 3 deals
            yield return new object[] { 4, 0, new Position { Table = 0, Deals = new[] { 13, 14, 15 }, North = 0, South = 1, East = 2, West = 3 } };
            yield return new object[] { 4, 4, new Position { Table = 1, Deals = new[] { 14, 15, 13 }, North = 4, South = 5, East = 10, West = 11 } };
            yield return new object[] { 4, 6, new Position { Table = 2, Deals = new[] { 15, 13, 14 }, North = 6, South = 7, East = 8, West = 9 } };
        }
    }

    [Theory]
    [MemberData(nameof(TriplicatePositions))]
    public void PositionsAreCorrect(int round, int player, Position expected)
    {
        var positions = _triplicate.GetPositions(3, 5, 3);
        var actual = positions[round][player];
        Assert.Equal(expected, actual, new PositionComparer());
    }

    public static IEnumerable<object[]> TriplicatePositions4Deals
    {
        get
        {
            // first serie of 4 deals
            yield return new object[] { 0, 0, new Position { Table = 0, Deals = new[] { 1, 2, 3, 4 }, North = 0, South = 1, East = 10, West = 11 } };
            yield return new object[] { 0, 2, new Position { Table = 1, Deals = new[] { 2, 3, 4, 1 }, North = 2, South = 3, East = 8, West = 9 } };
            yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 3, 4, 1, 2 }, North = 4, South = 5, East = 6, West = 7 } };
            // second serie of 4 deals
            yield return new object[] { 1, 0, new Position { Table = 0, Deals = new[] { 5, 6, 7, 8 }, North = 0, South = 1, East = 8, West = 9 } };
            yield return new object[] { 1, 10, new Position { Table = 1, Deals = new[] { 6, 7, 8, 5 }, North = 10, South = 11, East = 6, West = 7 } };
            yield return new object[] { 1, 2, new Position { Table = 2, Deals = new[] { 7, 8, 5, 6 }, North = 2, South = 3, East = 4, West = 5 } };
            // last serie of 4 deals
            yield return new object[] { 4, 0, new Position { Table = 0, Deals = new[] { 17, 18, 19, 20 }, North = 0, South = 1, East = 2, West = 3 } };
            yield return new object[] { 4, 4, new Position { Table = 1, Deals = new[] { 18, 19, 20, 17 }, North = 4, South = 5, East = 10, West = 11 } };
            yield return new object[] { 4, 6, new Position { Table = 2, Deals = new[] { 19, 20, 17, 18 }, North = 6, South = 7, East = 8, West = 9 } };
        }
    }

    [Theory]
    [MemberData(nameof(TriplicatePositions))]
    public void PositionsAreCorrect4Deals(int round, int player, Position expected)
    {
        var positions = _triplicate.GetPositions(3, 5, 3);
        var actual = positions[round][player];
        Assert.Equal(expected, actual, new PositionComparer());
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 1)]
    [InlineData(4, 2)]
    [InlineData(14, 3)]
    public void DealsAreCorrect(int index, int expectedEquivalent)
    {
        var deals = _triplicate.CreateDeals(3, 5, 3, 3);
        var deal = deals[index];
        Assert.Equal(index + 1, deal.Id);
        Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
        Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
    }

    [Fact]
    public void DealsScoresAreCorrect()
    {
        var deals = _triplicate.CreateDeals(3, 5, 6, 16);
        Assert.Equal(30, deals.Length);
        // all deals played exactly thrice, in the same round
        for (int i = 0; i < deals.Length; i++)
        {
            Assert.Equal(3, deals[i].Scores.Length);
            Assert.Equal(i / 6, deals[i].Scores[0].Round);
            Assert.Equal(i / 6, deals[i].Scores[1].Round);
            Assert.Equal(i / 6, deals[i].Scores[2].Round);
            Assert.Equal(0, deals[i].Scores[0].Table);
            Assert.Equal(1, deals[i].Scores[1].Table);
            Assert.Equal(2, deals[i].Scores[2].Table);
        }
    }
}

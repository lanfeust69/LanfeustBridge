using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using Models;

    public class TriplicateTest
    {
        private Triplicate _triplicate = new Triplicate();

        public static IEnumerable<object[]> TriplicatePositions
        {
            get
            {
                // first serie of 3 deals
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 10, West = 11 } };
                yield return new object[] { 0, 2, new Position { Table = 2, Deals = new[] { 2 }, North = 2, South = 3, East = 8, West = 9 } };
                yield return new object[] { 0, 4, new Position { Table = 3, Deals = new[] { 3 }, North = 4, South = 5, East = 6, West = 7 } };
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 2 }, North = 0, South = 1, East = 10, West = 11 } };
                yield return new object[] { 1, 2, new Position { Table = 2, Deals = new[] { 3 }, North = 2, South = 3, East = 8, West = 9 } };
                yield return new object[] { 1, 4, new Position { Table = 3, Deals = new[] { 1 }, North = 4, South = 5, East = 6, West = 7 } };
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 3 }, North = 0, South = 1, East = 10, West = 11 } };
                yield return new object[] { 2, 2, new Position { Table = 2, Deals = new[] { 1 }, North = 2, South = 3, East = 8, West = 9 } };
                yield return new object[] { 2, 4, new Position { Table = 3, Deals = new[] { 2 }, North = 4, South = 5, East = 6, West = 7 } };
                // second serie of 3 deals
                yield return new object[] { 3, 0, new Position { Table = 1, Deals = new[] { 4 }, North = 0, South = 1, East = 8, West = 9 } };
                yield return new object[] { 3, 10, new Position { Table = 2, Deals = new[] { 5 }, North = 10, South = 11, East = 6, West = 7 } };
                yield return new object[] { 3, 2, new Position { Table = 3, Deals = new[] { 6 }, North = 2, South = 3, East = 4, West = 5 } };
                // third serie of 3 deals
                yield return new object[] { 6, 0, new Position { Table = 1, Deals = new[] { 7 }, North = 0, South = 1, East = 6, West = 7 } };
                yield return new object[] { 6, 8, new Position { Table = 2, Deals = new[] { 8 }, North = 8, South = 9, East = 4, West = 5 } };
                yield return new object[] { 6, 10, new Position { Table = 3, Deals = new[] { 9 }, North = 10, South = 11, East = 2, West = 3 } };
                // fourth serie of 3 deals
                yield return new object[] { 9, 0, new Position { Table = 1, Deals = new[] { 10 }, North = 0, South = 1, East = 4, West = 5 } };
                yield return new object[] { 9, 6, new Position { Table = 2, Deals = new[] { 11 }, North = 6, South = 7, East = 2, West = 3 } };
                yield return new object[] { 9, 8, new Position { Table = 3, Deals = new[] { 12 }, North = 8, South = 9, East = 10, West = 11 } };
                // last serie of 3 deals
                yield return new object[] { 12, 0, new Position { Table = 1, Deals = new[] { 13 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 12, 4, new Position { Table = 2, Deals = new[] { 14 }, North = 4, South = 5, East = 10, West = 11 } };
                yield return new object[] { 12, 6, new Position { Table = 3, Deals = new[] { 15 }, North = 6, South = 7, East = 8, West = 9 } };
            }
        }

        [Theory]
        [MemberData("TriplicatePositions")]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _triplicate.GetPositions(3, 15, 1);
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
            var deals = _triplicate.CreateDeals(3, 15, 1);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }
    }
}

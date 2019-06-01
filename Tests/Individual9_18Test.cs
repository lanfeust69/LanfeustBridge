using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    public class Individual9_18Test
    {
        private Individual9_18 _individual = new Individual9_18();

        public static IEnumerable<object[]> IndividualPositions
        {
            get
            {
                // first serie of 4 deals, player 0 is bye
                yield return new object[] { 0, 0, new Position { Table = 3, Deals = new int[0], North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 0, 1, new Position { Table = 1, Deals = new[] { 1, 2 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 3, 4 }, North = 4, South = 8, East = 5, West = 7 } };
                yield return new object[] { 1, 0, new Position { Table = 3, Deals = new int[0], North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 1, 1, new Position { Table = 1, Deals = new[] { 3, 4 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 1, 4, new Position { Table = 2, Deals = new[] { 1, 2 }, North = 4, South = 8, East = 5, West = 7 } };
                // second serie of 4 deals, player 1 is bye
                yield return new object[] { 2, 1, new Position { Table = 3, Deals = new int[0], North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 5, 6 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 2, 3, new Position { Table = 2, Deals = new[] { 7, 8 }, North = 5, South = 6, East = 3, West = 8 } };
                yield return new object[] { 3, 1, new Position { Table = 3, Deals = new int[0], North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 3, 0, new Position { Table = 1, Deals = new[] { 7, 8 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 3, 3, new Position { Table = 2, Deals = new[] { 5, 6 }, North = 5, South = 6, East = 3, West = 8 } };
                // check all positions, as movement is not straightforward
                yield return new object[] { 4, 0, new Position { Table = 1, Deals = new[] { 9, 10 }, North = 0, South = 1, East = 5, West = 8 } };
                yield return new object[] { 4, 3, new Position { Table = 2, Deals = new[] { 11, 12 }, North = 3, South = 7, East = 4, West = 6 } };
                yield return new object[] { 6, 0, new Position { Table = 1, Deals = new[] { 13, 14 }, North = 4, South = 5, East = 6, West = 0 } };
                yield return new object[] { 6, 1, new Position { Table = 2, Deals = new[] { 15, 16 }, North = 7, South = 2, East = 8, West = 1 } };
                yield return new object[] { 8, 1, new Position { Table = 1, Deals = new[] { 17, 18 }, North = 5, South = 3, East = 7, West = 1 } };
                yield return new object[] { 8, 0, new Position { Table = 2, Deals = new[] { 19, 20 }, North = 8, South = 0, East = 6, West = 2 } };
                yield return new object[] { 10, 2, new Position { Table = 1, Deals = new[] { 21, 22 }, North = 3, South = 4, East = 8, West = 2 } };
                yield return new object[] { 10, 0, new Position { Table = 2, Deals = new[] { 23, 24 }, North = 6, South = 1, East = 7, West = 0 } };
                yield return new object[] { 12, 0, new Position { Table = 1, Deals = new[] { 25, 26 }, North = 7, South = 8, East = 0, West = 3 } };
                yield return new object[] { 12, 1, new Position { Table = 2, Deals = new[] { 27, 28 }, North = 1, South = 5, East = 2, West = 4 } };
                yield return new object[] { 14, 1, new Position { Table = 1, Deals = new[] { 29, 30 }, North = 8, South = 6, East = 1, West = 4 } };
                yield return new object[] { 14, 0, new Position { Table = 2, Deals = new[] { 31, 32 }, North = 2, South = 3, East = 0, West = 5 } };
                yield return new object[] { 16, 2, new Position { Table = 1, Deals = new[] { 33, 34 }, North = 6, South = 7, East = 2, West = 5 } };
                yield return new object[] { 16, 0, new Position { Table = 2, Deals = new[] { 35, 36 }, North = 0, South = 4, East = 1, West = 3 } };
            }
        }

        [Theory]
        [MemberData(nameof(IndividualPositions))]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(2, 18, 2);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 1)]
        [InlineData(5, 2)]
        [InlineData(35, 4)]
        public void DealsAreCorrect(int index, int expectedEquivalent)
        {
            var deals = _individual.CreateDeals(2, 18, 2);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }
    }
}

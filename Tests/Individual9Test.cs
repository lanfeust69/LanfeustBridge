using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using Models;

    public class Individual9Test
    {
        private Individual9 _individual = new Individual9();

        public static IEnumerable<object[]> IndividualPositions
        {
            get
            {
                // first serie of 3 deals, player 0 is bye
                yield return new object[] { 0, 0, new Position { Table = 3, Deals = new int[0], North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 0, 1, new Position { Table = 1, Deals = new[] { 1 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 2 }, North = 4, South = 8, East = 5, West = 7 } };
                yield return new object[] { 1, 0, new Position { Table = 3, Deals = new int[0], North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 1, 1, new Position { Table = 1, Deals = new[] { 2 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 1, 4, new Position { Table = 2, Deals = new[] { 3 }, North = 4, South = 8, East = 5, West = 7 } };
                yield return new object[] { 2, 0, new Position { Table = 3, Deals = new int[0], North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 2, 1, new Position { Table = 1, Deals = new[] { 3 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 2, 4, new Position { Table = 2, Deals = new[] { 1 }, North = 4, South = 8, East = 5, West = 7 } };
                // second serie of 3 deals, player 1 is bye
                yield return new object[] { 3, 1, new Position { Table = 3, Deals = new int[0], North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 3, 0, new Position { Table = 1, Deals = new[] { 4 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 3, 3, new Position { Table = 2, Deals = new[] { 5 }, North = 5, South = 6, East = 3, West = 8 } };
                yield return new object[] { 4, 1, new Position { Table = 3, Deals = new int[0], North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 4, 0, new Position { Table = 1, Deals = new[] { 5 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 4, 3, new Position { Table = 2, Deals = new[] { 6 }, North = 5, South = 6, East = 3, West = 8 } };
                yield return new object[] { 5, 1, new Position { Table = 3, Deals = new int[0], North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 5, 0, new Position { Table = 1, Deals = new[] { 6 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 5, 3, new Position { Table = 2, Deals = new[] { 4 }, North = 5, South = 6, East = 3, West = 8 } };
                // check all positions, as movement is not straightforward
                yield return new object[] { 6, 0, new Position { Table = 1, Deals = new[] { 7 }, North = 0, South = 1, East = 5, West = 8 } };
                yield return new object[] { 6, 3, new Position { Table = 2, Deals = new[] { 8 }, North = 3, South = 7, East = 4, West = 6 } };
                yield return new object[] { 9, 0, new Position { Table = 1, Deals = new[] { 10 }, North = 4, South = 5, East = 6, West = 0 } };
                yield return new object[] { 9, 1, new Position { Table = 2, Deals = new[] { 11 }, North = 7, South = 2, East = 8, West = 1 } };
                yield return new object[] { 12, 1, new Position { Table = 1, Deals = new[] { 13 }, North = 5, South = 3, East = 7, West = 1 } };
                yield return new object[] { 12, 0, new Position { Table = 2, Deals = new[] { 14 }, North = 8, South = 0, East = 6, West = 2 } };
                yield return new object[] { 15, 2, new Position { Table = 1, Deals = new[] { 16 }, North = 3, South = 4, East = 8, West = 2 } };
                yield return new object[] { 15, 0, new Position { Table = 2, Deals = new[] { 17 }, North = 6, South = 1, East = 7, West = 0 } };
                yield return new object[] { 18, 0, new Position { Table = 1, Deals = new[] { 19 }, North = 7, South = 8, East = 0, West = 3 } };
                yield return new object[] { 18, 1, new Position { Table = 2, Deals = new[] { 20 }, North = 1, South = 5, East = 2, West = 4 } };
                yield return new object[] { 21, 1, new Position { Table = 1, Deals = new[] { 22 }, North = 8, South = 6, East = 1, West = 4 } };
                yield return new object[] { 21, 0, new Position { Table = 2, Deals = new[] { 23 }, North = 2, South = 3, East = 0, West = 5 } };
                yield return new object[] { 24, 2, new Position { Table = 1, Deals = new[] { 25 }, North = 6, South = 7, East = 2, West = 5 } };
                yield return new object[] { 24, 0, new Position { Table = 2, Deals = new[] { 26 }, North = 0, South = 4, East = 1, West = 3 } };
            }
        }

        [Theory]
        [MemberData("IndividualPositions")]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(2, 27, 1);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(7, 8)]
        [InlineData(8, 1)]
        [InlineData(9, 2)]
        [InlineData(26, 3)]
        public void DealsAreCorrect(int index, int expectedEquivalent)
        {
            var deals = _individual.CreateDeals(2, 27, 1);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }
    }
}

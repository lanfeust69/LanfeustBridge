using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using Models;

    public class IndividualTest
    {
        private Individual _individual = new Individual();

        public static IEnumerable<object[]> IndividualPositions
        {
            get
            {
                // first serie of 3 deals
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1 }, North = 11, South = 0, East = 5, West = 7 } };
                yield return new object[] { 0, 1, new Position { Table = 2, Deals = new[] { 2 }, North = 1, South = 8, East = 9, West = 6 } };
                yield return new object[] { 0, 2, new Position { Table = 3, Deals = new[] { 3 }, North = 3, South = 2, East = 4, West = 10 } };
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 2 }, North = 11, South = 0, East = 5, West = 7 } };
                yield return new object[] { 1, 1, new Position { Table = 2, Deals = new[] { 3 }, North = 1, South = 8, East = 9, West = 6 } };
                yield return new object[] { 1, 2, new Position { Table = 3, Deals = new[] { 1 }, North = 3, South = 2, East = 4, West = 10 } };
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 3 }, North = 11, South = 0, East = 5, West = 7 } };
                yield return new object[] { 2, 1, new Position { Table = 2, Deals = new[] { 1 }, North = 1, South = 8, East = 9, West = 6 } };
                yield return new object[] { 2, 2, new Position { Table = 3, Deals = new[] { 2 }, North = 3, South = 2, East = 4, West = 10 } };
                // second serie of 3 deals
                yield return new object[] { 3, 1, new Position { Table = 1, Deals = new[] { 4 }, North = 11, South = 1, East = 6, West = 8 } };
                yield return new object[] { 3, 2, new Position { Table = 2, Deals = new[] { 5 }, North = 2, South = 9, East = 10, West = 7 } };
                yield return new object[] { 3, 3, new Position { Table = 3, Deals = new[] { 6 }, North = 4, South = 3, East = 5, West = 0 } };
                // last serie of 3 deals
                yield return new object[] { 32, 4, new Position { Table = 1, Deals = new[] { 33 }, North = 11, South = 10, East = 4, West = 6 } };
                yield return new object[] { 32, 0, new Position { Table = 2, Deals = new[] { 31 }, North = 0, South = 7, East = 8, West = 5 } };
                yield return new object[] { 32, 1, new Position { Table = 3, Deals = new[] { 32 }, North = 2, South = 1, East = 3, West = 9 } };
            }
        }

        [Theory]
        [MemberData("IndividualPositions")]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(3, 33, 1);
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
            var deals = _individual.CreateDeals(3, 33, 1);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }
    }
}

using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    public class Individual8Test
    {
        private readonly Individual8 _individual = new Individual8();

        public static IEnumerable<object[]> IndividualPositions
        {
            get
            {
                // first serie of 4 deals
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1, 2 }, North = 7, South = 0, East = 5, West = 1 } };
                yield return new object[] { 0, 2, new Position { Table = 2, Deals = new[] { 3, 4 }, North = 4, South = 6, East = 2, West = 3 } };
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 3, 4 }, North = 7, South = 0, East = 5, West = 1 } };
                yield return new object[] { 1, 2, new Position { Table = 2, Deals = new[] { 1, 2 }, North = 4, South = 6, East = 2, West = 3 } };
                // second serie of 4 deals
                yield return new object[] { 2, 0, new Position { Table = 2, Deals = new[] { 7, 8 }, North = 5, South = 0, East = 3, West = 4 } };
                yield return new object[] { 2, 1, new Position { Table = 1, Deals = new[] { 5, 6 }, North = 7, South = 1, East = 6, West = 2 } };
                yield return new object[] { 3, 0, new Position { Table = 2, Deals = new[] { 5, 6 }, North = 5, South = 0, East = 3, West = 4 } };
                yield return new object[] { 3, 1, new Position { Table = 1, Deals = new[] { 7, 8 }, North = 7, South = 1, East = 6, West = 2 } };
                // last serie of 4 deals
                yield return new object[] { 12, 0, new Position { Table = 1, Deals = new[] { 25, 26 }, North = 7, South = 6, East = 4, West = 0 } };
                yield return new object[] { 12, 1, new Position { Table = 2, Deals = new[] { 27, 28 }, North = 3, South = 5, East = 1, West = 2 } };
                yield return new object[] { 13, 0, new Position { Table = 1, Deals = new[] { 27, 28 }, North = 7, South = 6, East = 4, West = 0 } };
                yield return new object[] { 13, 1, new Position { Table = 2, Deals = new[] { 25, 26 }, North = 3, South = 5, East = 1, West = 2 } };
            }
        }

        [Theory]
        [MemberData(nameof(IndividualPositions))]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(2, 14, 2);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(7, 8)]
        [InlineData(8, 1)]
        [InlineData(9, 2)]
        [InlineData(27, 4)]
        public void DealsAreCorrect(int index, int expectedEquivalent)
        {
            var deals = _individual.CreateDeals(2, 14, 2);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }
    }
}

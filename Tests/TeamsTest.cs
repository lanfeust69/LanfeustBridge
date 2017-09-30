using System;
using System.Collections.Generic;

using Xunit;

using LanfeustBridge.Models;

namespace LanfeustBridge.Tests
{
    public class TeamsTest
    {
        Teams _teams = new Teams();

        public static IEnumerable<object[]> TeamsPositions
        {
            get
            {
                // first round
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1, 2 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 6, new Position { Table = 2, Deals = new[] { 3, 4 }, North = 4, South = 5, East = 6, West = 7 } };
                // second round : switch deals
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 3, 4 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 1, 6, new Position { Table = 2, Deals = new[] { 1, 2 }, North = 4, South = 5, East = 6, West = 7 } };
                // third round : begin second half
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 5, 6 }, North = 0, South = 1, East = 4, West = 5 } };
                yield return new object[] { 2, 6, new Position { Table = 2, Deals = new[] { 7, 8 }, North = 2, South = 3, East = 6, West = 7 } };
                // last round : switch deals
                yield return new object[] { 3, 0, new Position { Table = 1, Deals = new[] { 7, 8 }, North = 0, South = 1, East = 4, West = 5 } };
                yield return new object[] { 3, 6, new Position { Table = 2, Deals = new[] { 5, 6 }, North = 2, South = 3, East = 6, West = 7 } };
            }
        }

        [Theory]
        [MemberData("TeamsPositions")]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _teams.GetPositions(2, 4, 2);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Theory]
        [InlineData(1, 2, true, 0)]
        [InlineData(3, 2, true, 0)]
        [InlineData(4, 2, true, 0)]
        [InlineData(2, 3, true, 0)]
        [InlineData(2, 2, false, 6)]
        [InlineData(2, 8, false, 24)]
        public void DealsAreCorrect(int nbTables, int nbRounds, bool expectThrows, int expectedNumberOfDeals)
        {
            if (expectThrows)
                Assert.ThrowsAny<Exception>(() => _teams.CreateDeals(nbTables, nbRounds, 3));
            else
                Assert.Equal(expectedNumberOfDeals, _teams.CreateDeals(nbTables, nbRounds, 3).Length);
        }
    }
}

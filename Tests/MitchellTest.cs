using System;
using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using Models;

    public class MitchellTest
    {
        private Mitchell _mitchell = new Mitchell();

        public static IEnumerable<object[]> MitchellPositions3
        {
            get
            {
                // first position
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 1, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 2, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 3, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 2 }, North = 4, South = 5, East = 6, West = 7 } };
                yield return new object[] { 0, 8, new Position { Table = 3, Deals = new[] { 3 }, North = 8, South = 9, East = 10, West = 11 } };
                // second position
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 2 }, North = 0, South = 1, East = 10, West = 11 } };
                yield return new object[] { 1, 10, new Position { Table = 1, Deals = new[] { 2 }, North = 0, South = 1, East = 10, West = 11 } };
                yield return new object[] { 1, 4, new Position { Table = 2, Deals = new[] { 3 }, North = 4, South = 5, East = 2, West = 3 } };
                yield return new object[] { 1, 2, new Position { Table = 2, Deals = new[] { 3 }, North = 4, South = 5, East = 2, West = 3 } };
                yield return new object[] { 1, 8, new Position { Table = 3, Deals = new[] { 1 }, North = 8, South = 9, East = 6, West = 7 } };
                yield return new object[] { 1, 6, new Position { Table = 3, Deals = new[] { 1 }, North = 8, South = 9, East = 6, West = 7 } };
                // third position
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 3 }, North = 0, South = 1, East = 6, West = 7 } };
                yield return new object[] { 2, 6, new Position { Table = 1, Deals = new[] { 3 }, North = 0, South = 1, East = 6, West = 7 } };
                yield return new object[] { 2, 4, new Position { Table = 2, Deals = new[] { 1 }, North = 4, South = 5, East = 10, West = 11 } };
                yield return new object[] { 2, 10, new Position { Table = 2, Deals = new[] { 1 }, North = 4, South = 5, East = 10, West = 11 } };
                yield return new object[] { 2, 8, new Position { Table = 3, Deals = new[] { 2 }, North = 8, South = 9, East = 2, West = 3 } };
                yield return new object[] { 2, 2, new Position { Table = 3, Deals = new[] { 2 }, North = 8, South = 9, East = 2, West = 3 } };
            }
        }

        [Theory]
        [MemberData("MitchellPositions3")]
        public void PositionsAreCorrectFor3Tables(int round, int player, Position expected)
        {
            var positions = _mitchell.GetPositions(3, 3, 1);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Fact]
        public void CannotHaveAsManyRoundsAsTablesForEvenNumber()
        {
            Assert.ThrowsAny<Exception>(() => _mitchell.GetPositions(4, 4, 1));
        }

        #pragma warning disable SA1201 // Elements must appear in the correct order
        public static IEnumerable<object[]> MitchellPositions4
        {
            get
            {
                // first position
                yield return new object[] { 0, 0, new Position { Table = 1, Deals = new[] { 1 }, North = 0, South = 1, East = 2, West = 3 } };
                yield return new object[] { 0, 4, new Position { Table = 2, Deals = new[] { 2 }, North = 4, South = 5, East = 6, West = 7 } };
                yield return new object[] { 0, 8, new Position { Table = 3, Deals = new[] { 3 }, North = 8, South = 9, East = 10, West = 11 } };
                yield return new object[] { 0, 12, new Position { Table = 4, Deals = new[] { 4 }, North = 12, South = 13, East = 14, West = 15 } };
                // second position
                yield return new object[] { 1, 0, new Position { Table = 1, Deals = new[] { 2 }, North = 0, South = 1, East = 14, West = 15 } };
                yield return new object[] { 1, 4, new Position { Table = 2, Deals = new[] { 3 }, North = 4, South = 5, East = 2, West = 3 } };
                yield return new object[] { 1, 8, new Position { Table = 3, Deals = new[] { 4 }, North = 8, South = 9, East = 6, West = 7 } };
                yield return new object[] { 1, 12, new Position { Table = 4, Deals = new[] { 1 }, North = 12, South = 13, East = 10, West = 11 } };
                // third (and last) position : pairs skipped
                yield return new object[] { 2, 0, new Position { Table = 1, Deals = new[] { 3 }, North = 0, South = 1, East = 6, West = 7 } };
                yield return new object[] { 2, 4, new Position { Table = 2, Deals = new[] { 4 }, North = 4, South = 5, East = 10, West = 11 } };
                yield return new object[] { 2, 8, new Position { Table = 3, Deals = new[] { 1 }, North = 8, South = 9, East = 14, West = 15 } };
                yield return new object[] { 2, 12, new Position { Table = 4, Deals = new[] { 2 }, North = 12, South = 13, East = 2, West = 3 } };
            }
        }

        [Theory]
        [MemberData("MitchellPositions4")]
        public void PositionsAreCorrectFor4Tables(int round, int player, Position expected)
        {
            var positions = _mitchell.GetPositions(4, 3, 1);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        [Theory]
        [InlineData(1, 1, true, 0)]
        [InlineData(2, 1, true, 0)]
        [InlineData(3, 1, false, 9)]
        [InlineData(3, 2, false, 9)]
        [InlineData(3, 3, false, 9)]
        [InlineData(3, 4, true, 9)]
        [InlineData(4, 1, false, 12)]
        [InlineData(4, 2, false, 12)]
        [InlineData(4, 3, false, 12)]
        [InlineData(4, 4, true, 12)]
        [InlineData(5, 5, false, 15)]
        public void DealsAreCorrect(int nbTables, int nbRounds, bool expectThrows, int expectedNumberOfDeals)
        {
            if (expectThrows)
                Assert.ThrowsAny<Exception>(() => _mitchell.CreateDeals(nbTables, nbRounds, 3));
            else
                Assert.Equal(expectedNumberOfDeals, _mitchell.CreateDeals(nbTables, nbRounds, 3).Length);
        }
    }
}

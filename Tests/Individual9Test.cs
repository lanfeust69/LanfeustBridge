using System;
using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    public class Individual9Test
    {
        private readonly Individual9 _individual = new Individual9();

        public static IEnumerable<object[]> IndividualPositions
        {
            get
            {
                // first serie of 3 deals, player 0 is bye
                yield return new object[] { 0, 0, new Position { Table = 2, Deals = Array.Empty<int>(), North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 0, 1, new Position { Table = 0, Deals = new[] { 1, 2, 3 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 0, 4, new Position { Table = 1, Deals = new[] { 2, 3, 1 }, North = 4, South = 8, East = 5, West = 7 } };
                // second serie of 3 deals, player 1 is bye
                yield return new object[] { 1, 1, new Position { Table = 2, Deals = Array.Empty<int>(), North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 1, 0, new Position { Table = 0, Deals = new[] { 4, 5, 6 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 1, 3, new Position { Table = 1, Deals = new[] { 5, 6, 4 }, North = 5, South = 6, East = 3, West = 8 } };
                // check all positions, as movement is not straightforward
                yield return new object[] { 2, 0, new Position { Table = 0, Deals = new[] { 7, 8, 9 }, North = 0, South = 1, East = 5, West = 8 } };
                yield return new object[] { 2, 3, new Position { Table = 1, Deals = new[] { 8, 9, 7 }, North = 3, South = 7, East = 4, West = 6 } };
                yield return new object[] { 3, 0, new Position { Table = 0, Deals = new[] { 10, 11, 12 }, North = 4, South = 5, East = 6, West = 0 } };
                yield return new object[] { 3, 1, new Position { Table = 1, Deals = new[] { 11, 12, 10 }, North = 7, South = 2, East = 8, West = 1 } };
                yield return new object[] { 4, 1, new Position { Table = 0, Deals = new[] { 13, 14, 15 }, North = 5, South = 3, East = 7, West = 1 } };
                yield return new object[] { 4, 0, new Position { Table = 1, Deals = new[] { 14, 15, 13 }, North = 8, South = 0, East = 6, West = 2 } };
                yield return new object[] { 5, 2, new Position { Table = 0, Deals = new[] { 16, 17, 18 }, North = 3, South = 4, East = 8, West = 2 } };
                yield return new object[] { 5, 0, new Position { Table = 1, Deals = new[] { 17, 18, 16 }, North = 6, South = 1, East = 7, West = 0 } };
                yield return new object[] { 6, 0, new Position { Table = 0, Deals = new[] { 19, 20, 21 }, North = 7, South = 8, East = 0, West = 3 } };
                yield return new object[] { 6, 1, new Position { Table = 1, Deals = new[] { 20, 21, 19 }, North = 1, South = 5, East = 2, West = 4 } };
                yield return new object[] { 7, 1, new Position { Table = 0, Deals = new[] { 22, 23, 24 }, North = 8, South = 6, East = 1, West = 4 } };
                yield return new object[] { 7, 0, new Position { Table = 1, Deals = new[] { 23, 24, 22 }, North = 2, South = 3, East = 0, West = 5 } };
                yield return new object[] { 8, 2, new Position { Table = 0, Deals = new[] { 25, 26, 27 }, North = 6, South = 7, East = 2, West = 5 } };
                yield return new object[] { 8, 0, new Position { Table = 1, Deals = new[] { 26, 27, 25 }, North = 0, South = 4, East = 1, West = 3 } };
            }
        }

        [Theory]
        [MemberData(nameof(IndividualPositions))]
        public void PositionsAreCorrect(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(2, 9, 3);
            var actual = positions[round][player];
            Assert.Equal(expected, actual, new PositionComparer());
        }

        public static IEnumerable<object[]> IndividualPositions4Deals
        {
            get
            {
                // first serie of 4 deals, player 0 is bye
                yield return new object[] { 0, 0, new Position { Table = 2, Deals = Array.Empty<int>(), North = 0, South = 0, East = 0, West = 0 } };
                yield return new object[] { 0, 1, new Position { Table = 0, Deals = new[] { 1, 2, 3, 4 }, North = 1, South = 2, East = 3, West = 6 } };
                yield return new object[] { 0, 4, new Position { Table = 1, Deals = new[] { 3, 4, 1, 2 }, North = 4, South = 8, East = 5, West = 7 } };
                // second serie of 4 deals, player 1 is bye
                yield return new object[] { 1, 1, new Position { Table = 2, Deals = Array.Empty<int>(), North = 1, South = 1, East = 1, West = 1 } };
                yield return new object[] { 1, 0, new Position { Table = 0, Deals = new[] { 5, 6, 7, 8 }, North = 2, South = 0, East = 4, West = 7 } };
                yield return new object[] { 1, 3, new Position { Table = 1, Deals = new[] { 7, 8, 5, 6 }, North = 5, South = 6, East = 3, West = 8 } };
                // check all positions, as movement is not straightforward
                yield return new object[] { 2, 0, new Position { Table = 0, Deals = new[] { 9, 10, 11, 12 }, North = 0, South = 1, East = 5, West = 8 } };
                yield return new object[] { 2, 3, new Position { Table = 1, Deals = new[] { 11, 12, 9, 10 }, North = 3, South = 7, East = 4, West = 6 } };
                yield return new object[] { 3, 0, new Position { Table = 0, Deals = new[] { 13, 14, 15, 16 }, North = 4, South = 5, East = 6, West = 0 } };
                yield return new object[] { 3, 1, new Position { Table = 1, Deals = new[] { 15, 16, 13, 14 }, North = 7, South = 2, East = 8, West = 1 } };
                yield return new object[] { 4, 1, new Position { Table = 0, Deals = new[] { 17, 18, 19, 20 }, North = 5, South = 3, East = 7, West = 1 } };
                yield return new object[] { 4, 0, new Position { Table = 1, Deals = new[] { 19, 20, 17, 18 }, North = 8, South = 0, East = 6, West = 2 } };
                yield return new object[] { 5, 2, new Position { Table = 0, Deals = new[] { 21, 22, 23, 24 }, North = 3, South = 4, East = 8, West = 2 } };
                yield return new object[] { 5, 0, new Position { Table = 1, Deals = new[] { 23, 24, 21, 22 }, North = 6, South = 1, East = 7, West = 0 } };
                yield return new object[] { 6, 0, new Position { Table = 0, Deals = new[] { 25, 26, 27, 28 }, North = 7, South = 8, East = 0, West = 3 } };
                yield return new object[] { 6, 1, new Position { Table = 1, Deals = new[] { 27, 28, 25, 26 }, North = 1, South = 5, East = 2, West = 4 } };
                yield return new object[] { 7, 1, new Position { Table = 0, Deals = new[] { 29, 30, 31, 32 }, North = 8, South = 6, East = 1, West = 4 } };
                yield return new object[] { 7, 0, new Position { Table = 1, Deals = new[] { 31, 32, 29, 30 }, North = 2, South = 3, East = 0, West = 5 } };
                yield return new object[] { 8, 2, new Position { Table = 0, Deals = new[] { 33, 34, 35, 36 }, North = 6, South = 7, East = 2, West = 5 } };
                yield return new object[] { 8, 0, new Position { Table = 1, Deals = new[] { 35, 36, 33, 34 }, North = 0, South = 4, East = 1, West = 3 } };
            }
        }

        [Theory]
        [MemberData(nameof(IndividualPositions4Deals))]
        public void PositionsAreCorrect4Deals(int round, int player, Position expected)
        {
            var positions = _individual.GetPositions(2, 9, 4);
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
            var deals = _individual.CreateDeals(2, 9, 3);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }

        [Theory(Skip = "Need additional nbBoards argument to CreateDeals")]
        [InlineData(0, 1)]
        [InlineData(3, 4)]
        [InlineData(4, 1)]
        [InlineData(5, 2)]
        [InlineData(35, 4)]
        public void DealsAreCorrect4Deals(int index, int expectedEquivalent)
        {
            var deals = _individual.CreateDeals(2, 9, 4);
            var deal = deals[index];
            Assert.Equal(index + 1, deal.Id);
            Assert.Equal(Deal.ComputeDealer(expectedEquivalent), deal.Dealer);
            Assert.Equal(Deal.ComputeVulnerability(expectedEquivalent), deal.Vulnerability);
        }

        [Fact]
        public void DealsScoresAreCorrect()
        {
            var deals = _individual.CreateDeals(2, 9, 3);
            Assert.Equal(27, deals.Length);
            // all deals played exactly twice, in the same round
            for (int i = 0; i < deals.Length; i++)
            {
                Assert.Equal(2, deals[i].Scores.Length);
                Assert.Equal(i / 3, deals[i].Scores[0].Round);
                Assert.Equal(i / 3, deals[i].Scores[1].Round);
                Assert.Equal(0, deals[i].Scores[0].Table);
                Assert.Equal(1, deals[i].Scores[1].Table);
            }
        }

        [Fact]
        public void DealsScores4DealsAreCorrect()
        {
            var deals = _individual.CreateDeals(2, 9, 4);
            Assert.Equal(36, deals.Length);
            // all deals played exactly twice, in the same round
            for (int i = 0; i < deals.Length; i++)
            {
                Assert.Equal(2, deals[i].Scores.Length);
                Assert.Equal(i / 4, deals[i].Scores[0].Round);
                Assert.Equal(i / 4, deals[i].Scores[1].Round);
                Assert.Equal(0, deals[i].Scores[0].Table);
                Assert.Equal(1, deals[i].Scores[1].Table);
            }
        }
    }
}

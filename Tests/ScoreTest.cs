using System.Collections.Generic;

using Xunit;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    public class ScoreTest
    {
        public static IEnumerable<object[]> ContractScores
        {
            get
            {
                yield return new object[] { "NS", new Contract { Level = 0 }, 0, 0 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N" }, 6, -100 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E" }, 6, 50 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N" }, 7, 90 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E" }, 7, -90 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N" }, 9, 150 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N" }, 12, 240 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 3, -1100 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 3, 800 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 4, -800 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 4, 500 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 5, -500 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 5, 300 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 6, -200 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 6, 100 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 7, 180 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 7, -180 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 9, 580 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 9, -380 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 12, 1180 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 3, -2200 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 3, 1600 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 4, -1600 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 4, 1000 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 5, -1000 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 5, 600 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 6, -400 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 6, 200 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 7, 760 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 7, -560 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 9, 1560 };
                yield return new object[] { "NS", new Contract { Level = 1, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 13, 3160 };
                yield return new object[] { "NS", new Contract { Level = 2, Suit = Suit.Spades, Declarer = "N" }, 8, 110 };
                yield return new object[] { "NS", new Contract { Level = 2, Suit = Suit.Spades, Declarer = "E" }, 8, -110 };
                yield return new object[] { "NS", new Contract { Level = 2, Suit = Suit.Spades, Declarer = "N", Doubled = true }, 8, 670 };
                yield return new object[] { "NS", new Contract { Level = 2, Suit = Suit.Spades, Declarer = "E", Doubled = true }, 8, -470 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "N" }, 9, 600 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "E" }, 9, -400 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "N", Doubled = true }, 9, 750 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "E", Doubled = true }, 9, -550 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 9, 1000 };
                yield return new object[] { "NS", new Contract { Level = 3, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 9, -800 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "N" }, 10, 620 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "E" }, 10, -420 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "N" }, 11, 650 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "E" }, 11, -450 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "N", Doubled = true }, 10, 790 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "E", Doubled = true }, 10, -590 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "N", Redoubled = true }, 10, 1080 };
                yield return new object[] { "NS", new Contract { Level = 4, Suit = Suit.Spades, Declarer = "E", Redoubled = true }, 10, -880 };
                yield return new object[] { "NS", new Contract { Level = 5, Suit = Suit.Clubs, Declarer = "N" }, 11, 600 };
                yield return new object[] { "NS", new Contract { Level = 5, Suit = Suit.Clubs, Declarer = "E" }, 11, -400 };
                yield return new object[] { "NS", new Contract { Level = 5, Suit = Suit.Clubs, Declarer = "N" }, 13, 640 };
                yield return new object[] { "NS", new Contract { Level = 5, Suit = Suit.Clubs, Declarer = "E" }, 13, -440 };
                yield return new object[] { "NS", new Contract { Level = 6, Suit = Suit.Clubs, Declarer = "N" }, 12, 1370 };
                yield return new object[] { "NS", new Contract { Level = 6, Suit = Suit.Clubs, Declarer = "E" }, 12, -920 };
                yield return new object[] { "NS", new Contract { Level = 6, Suit = Suit.Clubs, Declarer = "N" }, 13, 1390 };
                yield return new object[] { "NS", new Contract { Level = 6, Suit = Suit.Clubs, Declarer = "E" }, 13, -940 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.Clubs, Declarer = "N" }, 13, 2140 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.Clubs, Declarer = "E" }, 13, -1440 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.NoTrump, Declarer = "N" }, 13, 2220 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.NoTrump, Declarer = "E" }, 13, -1520 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.NoTrump, Declarer = "N", Redoubled = true }, 13, 2980 };
                yield return new object[] { "NS", new Contract { Level = 7, Suit = Suit.NoTrump, Declarer = "E", Redoubled = true }, 13, -2280 };
            }
        }

        [Theory]
        [MemberData(nameof(ContractScores))]
        public void ScoresAreCorrectlyComputed(string vulnerability, Contract contract, int tricks, int expected)
        {
            var score = new Score { Vulnerability = vulnerability, Contract = contract, Tricks = tricks };
            Assert.Equal(expected, score.ComputeBridgeScore());
            score.BridgeScore = expected;
            score.Entered = true;
            Assert.True(score.Validate());
        }

        public static IEnumerable<object[]> DealScores
        {
            get
            {
                yield return new object[] { true, new[] { 140 }, new[] { (0.0, 0.0) } };
                yield return new object[] { false, new[] { 140 }, new[] { (50.0, 50.0) } };
                yield return new object[] { true, new[] { 170, 620 }, new[] { (-10.0, 10.0), (10.0, -10.0) } };
                yield return new object[] { false, new[] { 170, 620 }, new[] { (0.0, 100.0), (100.0, 0.0) } };
                yield return new object[] { true, new[] { -140, -150 }, new[] { (0.0, 0.0), (0.0, 0.0) } };
                yield return new object[] { false, new[] { -140, -150 }, new[] { (100.0, 0.0), (0.0, 100.0) } };
                yield return new object[] { true, new[] { 400, 400 }, new[] { (0.0, 0.0), (0.0, 0.0) } };
                yield return new object[] { false, new[] { 400, 400 }, new[] { (50.0, 50.0), (50.0, 50.0) } };
                yield return new object[] { true, new[] { 400, -50, -50, 420, 150 }, new[] { (6.25, -6.25), (-6.25, 6.25), (-6.25, 6.25), (7.0, -7.0), (-0.75, 0.75) } };
                yield return new object[] { false, new[] { 400, -50, -50, 420, 150 }, new[] { (75.0, 25.0), (12.5, 87.5), (12.5, 87.5), (100.0, 0.0), (50.0, 50.0) } };
            }
        }

        [Theory]
        [MemberData(nameof(DealScores))]
        public void ScoringIsCorrect(bool isImp, int[] scores, (double, double)[] expected)
        {
            Assert.Equal(scores.Length, expected.Length);
            var deal = Deal.CreateDeal(1, scores.Length);
            for (int i = 0; i < scores.Length; i++)
            {
                deal.Scores[i].Entered = true;
                deal.Scores[i].BridgeScore = scores[i];
            }
            deal.ComputeResults(isImp ? ScoringMethod.IMP : ScoringMethod.Matchpoint);
            Assert.Equal(expected.Length, deal.Scores.Length);
            for (int i = 0; i < scores.Length; i++)
            {
                var (ns, ew) = expected[i];
                Assert.Equal(ns, deal.Scores[i].NSResult, 5);
                Assert.Equal(ew, deal.Scores[i].EWResult, 5);
            }
        }
    }
}

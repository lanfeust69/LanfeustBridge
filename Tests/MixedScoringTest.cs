using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    public class MixedScoringTest
    {
        [Theory]
        [InlineData(0, 1, 10.0)]
        [InlineData(0, 6, 10.0)]
        [InlineData(0, 14, 10.0)]
        [InlineData(1, 1, 11.2)]
        [InlineData(1, 6, 10.5)]
        [InlineData(1, 14, 10.33)]
        [InlineData(2, 1, 12.29)]
        [InlineData(2, 6, 10.99)]
        [InlineData(2, 14, 10.66)]
        [InlineData(5, 1, 15.0)]
        [InlineData(5, 6, 12.33)]
        [InlineData(5, 14, 11.58)]
        [InlineData(10, 1, 18.09)]
        [InlineData(10, 6, 14.25)]
        [InlineData(10, 14, 12.97)]
        [InlineData(14, 1, 19.69)]
        [InlineData(15, 1, 20.0)]
        [InlineData(20, 6, 17.12)]
        [InlineData(20, 14, 15.26)]
        [InlineData(40, 6, 20.0)]
        [InlineData(40, 14, 18.41)]
        [InlineData(60, 14, 20.0)]
        [InlineData(0, 4, 10.00)]  // -> 4
        [InlineData(1, 4, 10.61)]  // -> 4.24
        [InlineData(2, 4, 11.20)]  // -> 4.48
        [InlineData(3, 4, 11.76)]  // -> 4.70
        [InlineData(6, 4, 13.28)]  // -> 5.31
        [InlineData(7, 4, 13.74)]  // -> 5.50
        [InlineData(10, 4, 15.00)] // -> 6
        [InlineData(12, 4, 15.74)] // -> 6.30
        [InlineData(13, 4, 16.09)] // -> 6.44
        [InlineData(14, 4, 16.42)] // -> 6.57
        [InlineData(19, 4, 17.84)] // -> 7.14
        [InlineData(20, 4, 18.09)] // -> 7.24
        [InlineData(23, 4, 18.76)] // -> 7.50
        [InlineData(29, 4, 19.85)] // -> 7.94
        [InlineData(30, 4, 20.00)] // -> 8
        public void VpScoringIsCorrect(int imps, int nbDeals, double expected)
        {
            double vps = Tournament.ComputeVps(imps, nbDeals);
            Assert.Equal(expected, vps, 2);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SampleMixedScoring(bool tieAt10)
        {
            var tournament = new Tournament
            {
                Movement = "teams",
                NbTables = 2,
                Scoring = tieAt10 ? ScoringMethod.MixedTiedAt10 : ScoringMethod.Mixed,
                NbRounds = 2,
                NbDealsPerRound = 4,
                Players = Enumerable.Range(0, 8)
                    .Select(i => new Player { Name = $"Player {i + 1}" }).ToArray()
            };
            tournament.GeneratePositions();
            var deals = tournament.CreateDeals();
            Assert.Equal(8, deals.Length);
            // same scenario for both rounds
            for (int round = 0; round < 2; round++)
            {
                // first deal tied
                deals[round * 4].Scores[0] = new Score { Round = round, BridgeScore = -90, Entered = true };
                deals[round * 4].Scores[1] = new Score { Round = round, Table = 1, BridgeScore = -90, Entered = true };
                deals[round * 4].ComputeResults(tournament.Scoring);
                // second deal tied or not depending on scoring
                deals[round * 4 + 1].Scores[0] = new Score { Round = round, BridgeScore = 120, Entered = true };
                deals[round * 4 + 1].Scores[1] = new Score { Round = round, Table = 1, BridgeScore = 110, Entered = true };
                deals[round * 4 + 1].ComputeResults(tournament.Scoring);
                // third deal : EW win 2 IMPs
                deals[round * 4 + 2].Scores[0] = new Score { Round = round, BridgeScore = 400, Entered = true };
                deals[round * 4 + 2].Scores[1] = new Score { Round = round, Table = 1, BridgeScore = 450, Entered = true };
                deals[round * 4 + 2].ComputeResults(tournament.Scoring);
                // fourth deal : NS win 12 IMPs
                deals[round * 4 + 3].Scores[0] = new Score { Round = round, BridgeScore = 600, Entered = true };
                deals[round * 4 + 3].Scores[1] = new Score { Round = round, Table = 1, BridgeScore = -100, Entered = true };
                deals[round * 4 + 3].ComputeResults(tournament.Scoring);
            }
            tournament.Close(deals);
            // as both rounds are identical, we get the same average as a single one :
            // NS win 2 or 2.5 matchpoints out of 4 (depending on tie at 10),
            // and +10 IMPs on 4 deals converts to 15 VP on a 20 scale, so 3 out of 4 "IMPS-points"
            var expectedNS = ((tieAt10 ? 2.0 : 2.5) + 3.0) / 8.0 * 100.0;
            var expectedEW = 100.0 - expectedNS;
            Assert.Equal(expectedNS, tournament.Players[0].Score);
            Assert.Equal(expectedNS, tournament.Players[1].Score);
            Assert.Equal(expectedEW, tournament.Players[2].Score);
            Assert.Equal(expectedEW, tournament.Players[3].Score);
            Assert.Equal(expectedEW, tournament.Players[4].Score);
            Assert.Equal(expectedEW, tournament.Players[5].Score);
            Assert.Equal(expectedNS, tournament.Players[6].Score);
            Assert.Equal(expectedNS, tournament.Players[7].Score);
        }
    }
}

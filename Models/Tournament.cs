using System;
using System.Collections.Generic;
using System.Linq;

using LanfeustBridge.Services;

namespace LanfeustBridge.Models
{
    public enum TournamentStatus
    {
        Setup,
        Running,
        Finished
    }

    public static class ScoringMethod
    {
        public const string Matchpoint = "Matchpoint";
        public const string IMP = "IMP";
        public const string Mixed = "Mixed";
        public const string MixedTiedAt10 = "Mixed Tied at 10";
    }

    // built on the front-end side, assume non-nullable fields are OK
    public class Tournament
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTimeOffset Date { get; set; }

#pragma warning disable CA1721
        public string Movement { get; set; } = default!;
#pragma warning restore CA1721

        public string Scoring { get; set; } = default!;

        public int NbTables { get; set; }

        public int NbRounds { get; set; }

        public int NbDealsPerRound { get; set; }

        public int NbDeals { get; set; }

        // Number of boards used (deals will cycle through this number)
        public int NbBoards { get; set; } = 16;

        public Player[] Players { get; set; } = default!;

        public TournamentStatus Status { get; set; }

        public int CurrentRound { get; set; }

        // indexed by round, player
        public Position[][]? Positions { get; set; }

        public IMovement GetMovement() =>
            MovementService.Service.GetMovement(Movement) ?? throw new InvalidOperationException($"Unknown movement {Movement}");

        internal void GeneratePositions()
        {
            var movement = GetMovement();
            Positions = movement.GetPositions(NbTables, NbRounds, NbDealsPerRound);
        }

        internal Deal[] CreateDeals()
        {
            var movement = GetMovement();
            var deals = movement.CreateDeals(NbTables, NbRounds, NbDealsPerRound, NbBoards);
            NbDeals = deals.Length;
            return deals;
        }

        internal void Close(Deal[] deals)
        {
            Status = TournamentStatus.Finished;
            var players = new Dictionary<string, int>();
            for (int i = 0; i < Players.Length; i++)
            {
                players[Players[i].Name] = i;
                Players[i].Score = 0;
            }

            if (Scoring == ScoringMethod.Mixed || Scoring == ScoringMethod.MixedTiedAt10)
            {
                ProcessMixed(deals);
            }
            else
            {
                int[] nbPlayed = new int[Players.Length];
                foreach (var deal in deals)
                {
                    foreach (var score in deal.Scores)
                    {
                        if (!score.Entered)
                            continue;
                        int[] ids = new[] { score.Players.North, score.Players.South, score.Players.East, score.Players.West }
                            .Select(name => players[name]).ToArray();
                        for (int i = 0; i < 4; i++)
                        {
                            int player = ids[i];
                            nbPlayed[player]++;
                            Players[player].Score += i < 2 ? score.NSResult : score.EWResult;
                        }
                    }
                }

                if (Scoring == ScoringMethod.Matchpoint)
                {
                    for (int i = 0; i < Players.Length; i++)
                        Players[i].Score /= nbPlayed[i];
                }
            }

            var ranks = Enumerable.Range(0, Players.Length).OrderByDescending(i => Players[i].Score);
            var currentRank = 1;
            foreach (var rank in ranks)
                Players[rank].Rank = currentRank++;
        }

        internal bool AreAllScoresEntered(Deal[]? deals)
        {
            if (Status != TournamentStatus.Running || deals == null || Positions == null)
                return false;

            foreach (var position in Positions[CurrentRound].GroupBy(p => p.Table).Select(g => g.First()))
            {
                foreach (var dealId in position.Deals)
                {
                    // dealId is 1-based for display
                    if (deals[dealId - 1].Scores.All(s => s.Round != CurrentRound || s.Table != position.Table || !s.Entered))
                        return false;
                }
            }
            return true;
        }

        internal void ProcessMixed(Deal[] deals)
        {
            if (Positions == null)
                throw new InvalidOperationException("Positions have not been generated"); 

            // for each position, give each player a score based on the deals they played during this position :
            // - a standard matchpoint score (sum of matchpoints earned)
            // - an imp score, computed similarly to the WBF victory point scale, normalized so that 20VPs -> max possible of matchpoints
            bool tieAt10 = Scoring == ScoringMethod.MixedTiedAt10;
            var totalAvailable = new double[Players.Length];
            for (int round = 0; round < Positions.Length; round++)
            {
                for (int player = 0; player < Players.Length; player++)
                {
                    var position = Positions[round][player];
                    if (position.Deals.Length == 0)
                        continue;

                    bool isNS = position.North == player || position.South == player;
                    int sign = isNS ? 1 : -1;
                    double availableMatchpoints = 0;
                    double matchpoints = 0;
                    double imps = 0;
                    foreach (var dealId in position.Deals)
                    {
                        var deal = deals[dealId - 1];
                        availableMatchpoints += deal.Scores.Length - 1;
                        var score = deal.Scores.Single(s => s.Round == round && s.Table == position.Table);
                        // deals already imp-computed
                        imps += isNS ? score.NSResult : score.EWResult;
                        int bridgeScore = score.BridgeScore * sign;
                        foreach (var other in deal.Scores)
                        {
                            if (other == score)
                                continue;
                            if (tieAt10)
                            {
                                if (bridgeScore > other.BridgeScore * sign + 10)
                                    matchpoints += 1.0;
                                else if (bridgeScore >= other.BridgeScore * sign - 10)
                                    matchpoints += 0.5;
                            }
                            else
                            {
                                if (bridgeScore > other.BridgeScore * sign)
                                    matchpoints += 1.0;
                                else if (bridgeScore == other.BridgeScore * sign)
                                    matchpoints += 0.5;
                            }
                        }
                    }
                    double total = matchpoints + ComputeVps(imps, position.Deals.Length) * availableMatchpoints / 20.0;
                    Players[player].Score += total;
                    totalAvailable[player] += availableMatchpoints * 2;
                }
            }

            // finally convert as a percentage of available points
            for (int player = 0; player < Players.Length; player++)
            {
                Players[player].Score *= 100.0 / totalAvailable[player];
            }
        }

        internal static double ComputeVps(double imps, int nbDeals)
        {
            if (imps == 0)
                return 10.0;
            if (imps < 0)
                return 20.0 - ComputeVps(-imps, nbDeals);
            
            double phi = (Math.Sqrt(5) - 1) / 2;
            double b = 15 * Math.Sqrt(nbDeals);
            if (imps >= b)
                return 20.0;

            return 10.0 + 10.0 * (1 - Math.Pow(phi, 3 * imps / b)) / (1 - phi * phi * phi);
        }
    }
}

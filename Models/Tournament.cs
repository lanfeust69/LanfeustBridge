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

    // built on the front-end side, assume non-nullable fields are OK
    public class Tournament
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTimeOffset Date { get; set; }

        public string Movement { get; set; } = default!;

        public string Scoring { get; set; } = default!;

        public int NbTables { get; set; }

        public int NbRounds { get; set; }

        public int NbDealsPerRound { get; set; }

        public int NbDeals { get; set; }

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
            var deals = movement.CreateDeals(NbTables, NbRounds, NbDealsPerRound);
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

            if (Scoring == "Matchpoint")
            {
                for (int i = 0; i < Players.Length; i++)
                    Players[i].Score /= nbPlayed[i];
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

            foreach (var dealId in Positions[CurrentRound].SelectMany(p => p.Deals).Distinct())
            {
                // dealId is 1-based for display
                if (!deals[dealId - 1].Scores[CurrentRound].Entered)
                    return false;
            }
            return true;
        }
    }
}

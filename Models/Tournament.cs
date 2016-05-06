using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public enum TournamentStatus
    {
        Setup,
        Running,
        Finished
    }

    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Movement { get; set; }
        public string Scoring { get; set; }
        public int NbTables { get; set; }
        public int NbRounds { get; set; }
        public int NbDealsPerRound { get; set; }
        public int NbDeals { get; set; }
        public Player[] Players { get; set; }

        public TournamentStatus Status { get; set; }
        public int CurrentRound { get; set; }
        // indexed by round, player
        public Position[][] Positions { get; set; }

        public IMovement GetMovement()
        {
            switch (Movement)
            {
                case "Mitchell":
                    return new Mitchell();
                case "Teams":
                    return new Teams();
                case "Triplicate":
                    return new Triplicate();
                case "Individual":
                    return new Individual();
                default:
                    throw new NotImplementedException($"Movement {Movement} not implemented yet");
            }
        }

        internal void GeneratePositions()
        {
            IMovement movement = GetMovement();
            Positions = movement.GetPositions(NbTables, NbRounds, NbDealsPerRound);
        }

        internal Deal[] CreateDeals()
        {
            IMovement movement = GetMovement();
            var deals = movement.CreateDeals(NbTables, NbRounds, NbDealsPerRound);
            NbDeals = deals.Length;
            return deals;
        }

        internal void Close(Deal[] deals)
        {
            Status = TournamentStatus.Finished;
            var players = new Dictionary<string, int>();
            for (int i = 0; i < Players.Length; i++)
                players[Players[i].Name] = i;
            int[] nbPlayed = new int[Players.Length];
            foreach (var deal in deals)
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
    }
}

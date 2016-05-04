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
        public List<Player> Players { get; set; }

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
    }
}

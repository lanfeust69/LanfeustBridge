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
        public int NbTables { get; set; }
        public int NbRounds { get; set; }
        public List<Player> Players { get; set; }
        public List<Deal> Deals { get; set; }

        public TournamentStatus Status { get; set; }
        // indexed by table, round, boardInRound
        public List<List<Position>> Positions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Dealer { get; set; }
        public string Vulnerability { get; set; }
        public Hands Hands { get; set; }
        public Score[] Scores { get; set; }
        
        public static Deal CreateDeal(int id, int nbRounds)
        {
            var deal = new Deal
                {
                    Id = id,
                    Dealer = ComputeDealer(id),
                    Vulnerability = ComputeVulnerability(id),
                    Hands = new Hands(),
                    Scores = new Score[nbRounds]
                };
            for (int i = 0; i < nbRounds; i++)
                deal.Scores[i] = new Score { DealId = id, Round = i, Contract = new Contract() };
            return deal;
        }

        private static readonly string[] _players = { "N", "E", "S", "W" };
        public static string ComputeDealer(int id) {
            return _players[(id - 1) % 4];
        }

        public static string ComputeVulnerability(int id) {
            switch ((id - 1) % 16) {
                case 0: case 7: case 10: case 13:
                    return "None";
                case 1: case 4: case 11: case 14:
                    return "NS";
                case 2: case 5: case 8: case 15:
                    return "EW";
                case 3: case 6: case 9: case 12:
                    return "Both";
            }
            return "Unknown";
        }

        internal void ComputeResults(string scoring)
        {
        }
    }
}

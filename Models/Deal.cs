using System;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Dealer { get; set; }
        public string Vulnerability { get; set; }
        public Hands Hands { get; set; }
        public Score[] Scores { get; set; }
        
        public static Deal CreateDeal(int id, int nbRounds, string dealer = null, string vulnerability = null)
        {
            vulnerability = vulnerability ?? ComputeVulnerability(id);
            var deal = new Deal
                {
                    Id = id,
                    Dealer = dealer ?? ComputeDealer(id),
                    Vulnerability = vulnerability,
                    Hands = new Hands(),
                    Scores = new Score[nbRounds]
                };
            for (int i = 0; i < nbRounds; i++)
                deal.Scores[i] = new Score { DealId = id, Vulnerability = vulnerability, Round = i, Contract = new Contract() };
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
            switch (scoring)
            {
                case "Matchpoint":
                    ComputePoints(isImp: false);
                    break;
                case "IMP":
                    ComputePoints(isImp: true);
                    break;
                default:
                    throw new NotImplementedException($"Scoring method '{scoring}' not implemented yet");
            }
        }

        internal void ComputePoints(bool isImp)
        {
            var knownScores = Scores.Where(s => s.Entered).OrderBy(s => s.BridgeScore).ToList();
            foreach (var score in knownScores)
            {
                if (knownScores.Count < 2)
                {
                    score.NSResult = isImp ? 0 : 50;
                    score.EWResult = isImp ? 0 : 50;
                    continue;
                }
                double nsScore = 0;
                foreach (var other in knownScores)
                {
                    if (other == score)
                        continue;
                    nsScore += isImp
                        ? ConvertToImps(score.BridgeScore - other.BridgeScore)
                        : score.BridgeScore > other.BridgeScore ? 1 : (score.BridgeScore == other.BridgeScore ? 0.5 : 0);
                }
                score.NSResult = nsScore / (knownScores.Count - 1) * (isImp ? 1 : 100);
                score.EWResult = isImp ? -score.NSResult : 100 - score.NSResult;
            }
        }
        
        private static int[] _limits =
        {
            20, 50, 90, 130, 170, 220, 270, 320, 370, 430, 500, 600, 750, 900,
            1100, 1300, 1500, 1750, 2000, 2250, 2500, 2750, 3000, 3250, 3500
        };
        private int ConvertToImps(int diff)
        {
            if (diff < 0)
                return -ConvertToImps(-diff);
            for (int i = _limits.Length - 1; i >= 0; i--)
                if (diff >= _limits[i])
                    return i + 1;
            return 0;
        }
    }
}

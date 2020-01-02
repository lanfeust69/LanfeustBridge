using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Triplicate : IMovement
    {
        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 2,
            Id = typeof(Triplicate).Name.ToLower(),
            Name = "Triplicate for 6 pairs",
            Description = "Only accepts 5 rounds, playing against each of the other pairs",
            MinTables = 3,
            MaxTables = 3,
            MinRounds = 5,
            MaxRounds = 5
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < 3; table++)
                {
                    var position = new Position { Table = table };
                    int firstDeal = round * nbDealsPerRound + 1;
                    int offset = table * nbDealsPerRound / nbTables;
                    position.Deals = Enumerable.Range(0, nbDealsPerRound).Select(i => firstDeal + (offset + i) % nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 0 : (table == 1 ? ((5 - round) % 5 + 1) * 2 : ((6 - round) % 5 + 1) * 2);
                    position.South = table == 0 ? 1 : (table == 1 ? ((5 - round) % 5 + 1) * 2 + 1 : ((6 - round) % 5 + 1) * 2 + 1);
                    position.East = table == 0 ? ((9 - round) % 5 + 1) * 2 : (table == 1 ? ((8 - round) % 5 + 1) * 2 : ((7 - round) % 5 + 1) * 2);
                    position.West = table == 0 ? ((9 - round) % 5 + 1) * 2 + 1 : (table == 1 ? ((8 - round) % 5 + 1) * 2 + 1 : ((7 - round) % 5 + 1) * 2 + 1);
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound, int nbBoards)
        {
            CheckValidity(nbTables, nbRounds);
            if (nbDealsPerRound > nbBoards)
                throw new NotSupportedException($"Need at least {nbDealsPerRound} boards");

            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                // specify dealer and vulnerability : we keep playing deals 1 to nbDealsPerRound
                var deal = Deal.CreateDeal(i + 1, 3, Deal.ComputeDealer(i % nbBoards + 1), Deal.ComputeVulnerability(i % nbBoards + 1));
                deal.Scores[0].Round = deal.Scores[1].Round = deal.Scores[2].Round = i / nbDealsPerRound;
                deal.Scores[0].Table = 0;
                deal.Scores[1].Table = 1;
                deal.Scores[2].Table = 2;
                deals[i] = deal;
            }
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            if (nbTables != 3)
                reasons.Add("Only three tables allowed for triplicates");
            if (nbRounds != 5)
                reasons.Add("Only 5 rounds are allowed for triplicates");
            return new MovementValidation { IsValid = reasons.Count == 0, Reason = string.Join(" ; ", reasons) };
        }

        private void CheckValidity(int nbTables, int nbRounds)
        {
            var validity = Validate(nbTables, nbRounds);
            if (!validity.IsValid)
                throw new NotSupportedException(validity.Reason);
        }
    }
}

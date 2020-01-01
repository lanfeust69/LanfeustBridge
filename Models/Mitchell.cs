using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Mitchell : IMovement
    {
        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 0,
            Id = typeof(Mitchell).Name.ToLower(),
            Name = "Mitchell",
            Description = "Standard Mitchell : NS fixed, EW move to next table, boards to previous",
            MinTables = 3,
            MinRounds = 2
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < nbTables; table++)
                {
                    var position = new Position { Table = table };
                    int firstDeal = ((table + round) % nbTables) * nbDealsPerRound + 1;
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table * 4;
                    position.South = table * 4 + 1;
                    var ewTable = table - round - (nbTables % 2 == 0 && round >= nbTables / 2 ? 1 : 0);
                    if (ewTable < 0)
                        ewTable += nbTables;
                    position.East = ewTable * 4 + 2;
                    position.West = ewTable * 4 + 3;
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            int nbDeals = nbTables * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                var deal = Deal.CreateDeal(i + 1, nbRounds);
                for (int j = 0; j < nbRounds; j++)
                {
                    deal.Scores[j].Round = j;
                    deal.Scores[j].Table = (i / nbDealsPerRound + nbTables - j) % nbTables;
                }
                deals[i] = deal;
            }
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            if (nbTables < 3)
                reasons.Add("At least 3 tables needed for Mitchell");
            if (nbRounds > nbTables - (nbTables + 1) % 2)
                reasons.Add($"At most {nbTables - (nbTables + 1) % 2} rounds possible for a {nbTables} Mitchell");
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

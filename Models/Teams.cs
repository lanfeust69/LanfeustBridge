using System;
using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Teams : IMovement
    {
        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Order = 1,
            Id = typeof(Teams).Name.ToLower(),
            Name = "Teams match",
            Description = "Pairs are fixed for 2 rounds, where deals are switched, then pairs switch for next set of deals",
            MinTables = 2,
            MaxTables = 2,
            MinRounds = 2
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            // each odd round : switch boards, each even round : switch one team, next set of boards
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < 2; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round - round % 2 + (round + table) % 2) * nbDealsPerRound + 1;
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 0 : ((round / 2) % 2 == 0 ? 4 : 2);
                    position.South = table == 0 ? 1 : ((round / 2) % 2 == 0 ? 5 : 3);
                    position.East = table == 1 ? 6 : ((round / 2) % 2 == 0 ? 2 : 4);
                    position.West = table == 1 ? 7 : ((round / 2) % 2 == 0 ? 3 : 5);
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        private void CheckValidity(int nbTables, int nbRounds)
        {
            var validity = Validate(nbTables, nbRounds);
            if (!validity.IsValid)
                throw new NotSupportedException(validity.Reason);
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
                deals[i] = Deal.CreateDeal(i + 1, nbRounds);
            return deals;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            if (nbTables != 2)
                reasons.Add("Only two tables allowed for team matches");
            if (nbRounds % 2 != 0)
                reasons.Add("Only an even number of rounds are allowed for team matches");
            return new MovementValidation { IsValid = reasons.Count == 0, Reason = string.Join(" ; ", reasons) };
        }
    }
}

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
            Description = "Pairs switch for each set of deals",
            MinTables = 2,
            MaxTables = 2,
            MinRounds = 1
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
                    var position = new Position { Table = table };
                    int firstDeal = round * nbDealsPerRound + 1;
                    int offset = table * nbDealsPerRound / 2;
                    position.Deals = Enumerable.Range(0, nbDealsPerRound).Select(i => firstDeal + (offset + i) % nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 0 : (round % 2 == 0 ? 4 : 2);
                    position.South = table == 0 ? 1 : (round % 2 == 0 ? 5 : 3);
                    position.East = table == 1 ? 6 : (round % 2 == 0 ? 2 : 4);
                    position.West = table == 1 ? 7 : (round % 2 == 0 ? 3 : 5);
                    positions[position.North] = position;
                    positions[position.South] = position;
                    positions[position.East] = position;
                    positions[position.West] = position;
                }
                allPositions[round] = positions;
            }
            return allPositions;
        }

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            var reasons = new List<string>();
            if (nbTables != 2)
                reasons.Add("Only two tables allowed for team matches");
            return new MovementValidation { IsValid = reasons.Count == 0, Reason = string.Join(" ; ", reasons) };
        }

        public Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            CheckValidity(nbTables, nbRounds);
            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                var deal = Deal.CreateDeal(i + 1, nbScores: 2);
                deal.Scores[0].Round = deal.Scores[1].Round = i / nbDealsPerRound;
                deal.Scores[0].Table = 0;
                deal.Scores[1].Table = 1;
                deals[i] = deal;
            }
            return deals;
        }

        private void CheckValidity(int nbTables, int nbRounds)
        {
            var validity = Validate(nbTables, nbRounds);
            if (!validity.IsValid)
                throw new NotSupportedException(validity.Reason);
        }
    }
}

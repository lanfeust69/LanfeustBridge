using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Individual : IMovement
    {
        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            if (nbTables != 3)
                throw new NotSupportedException("Only three tables allowed for individuals");
            if (nbRounds != 33)
                throw new NotSupportedException("Only 33 rounds are allowed for individuals");
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            // each round : switch one team
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var playerRound = round / 3;
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < 3; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round - (round % 3) + ((round % 3) + table) % 3) * nbDealsPerRound + 1; 
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 11 : (table == 1 ? (playerRound + 1) % 11 : (playerRound + 3) % 11);
                    position.South = table == 0 ? playerRound : (table == 1 ? (playerRound + 8) % 11 : (playerRound + 2) % 11);
                    position.East = table == 0 ? (playerRound + 5) % 11 : (table == 1 ? (playerRound + 9) % 11 : (playerRound + 4) % 11);
                    position.West = table == 0 ? (playerRound + 7) % 11 : (table == 1 ? (playerRound + 6) % 11 : (playerRound + 10) % 11);
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
            if (nbTables != 3)
                throw new NotSupportedException("Only three tables allowed for individuals");
            if (nbRounds != 33)
                throw new NotSupportedException("Only 33 rounds are allowed for individuals");
            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
                deals[i] = Deal.CreateDeal(i + 1, nbRounds, Deal.ComputeDealer(i % 8 + 1), Deal.ComputeVulnerability(i % 8 + 1));
            return deals;
        }
    }
}
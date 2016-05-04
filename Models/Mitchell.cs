using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Mitchell : IMovement
    {
        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < nbTables; table++)
                {
                    var position = new Position { Table = table + 1 };
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
            int nbDeals = nbTables * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
                deals[i] = Deal.CreateDeal(i + 1, nbRounds);
            return deals;
        }
    }
}

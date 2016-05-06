using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Triplicate : IMovement
    {
        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            if (nbTables != 3)
                throw new NotSupportedException("Only three tables allowed for triplicates");
            if (nbRounds != 15)
                throw new NotSupportedException("Only 15 rounds are allowed for triplicates");
            // player ids : 0 = N1, 1 = S1, 2 = E1, 3 = W1, etc...
            var allPositions = new Position[nbRounds][];
            for (int round = 0; round < nbRounds; round++)
            {
                var positions = new Position[nbTables * 4];
                for (int table = 0; table < 3; table++)
                {
                    var position = new Position { Table = table + 1 };
                    int firstDeal = (round / 3 * 3 + ((round % 3) + table) % 3) * nbDealsPerRound + 1; 
                    position.Deals = Enumerable.Range(firstDeal, nbDealsPerRound).ToArray();
                    position.North = table == 0 ? 0 : (table == 1 ? ((5 - round / 3) % 5 + 1) * 2 : ((6 - round / 3) % 5 + 1) * 2);
                    position.South = table == 0 ? 1 : (table == 1 ? ((5 - round / 3) % 5 + 1) * 2 + 1 : ((6 - round / 3) % 5 + 1) * 2 + 1);
                    position.East = table == 0 ? ((9 - round / 3) % 5 + 1) * 2 : (table == 1 ? ((8 - round / 3) % 5 + 1) * 2 : ((7 - round / 3) % 5 + 1) * 2);
                    position.West = table == 0 ? ((9 - round / 3) % 5 + 1) * 2 + 1 : (table == 1 ? ((8 - round / 3) % 5 + 1) * 2 + 1 : ((7 - round / 3) % 5 + 1) * 2 + 1);
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
                throw new NotSupportedException("Only three tables allowed for triplicates");
            if (nbRounds != 15)
                throw new NotSupportedException("Only 15 rounds are allowed for triplicates");
            int nbDeals = nbRounds * nbDealsPerRound;
            var deals = new Deal[nbDeals];
            for (int i = 0; i < nbDeals; i++)
            {
                // specify dealer and vulnerability : we keep playing deals 1 to (3 * nbDealsPerRound)
                var deal = Deal.CreateDeal(i + 1, nbRounds, Deal.ComputeDealer(i % (3 * nbDealsPerRound) + 1), Deal.ComputeVulnerability(i % (3 * nbDealsPerRound) + 1));
                deals[i] = deal;
                
            }
            return deals;
        }
    }
}

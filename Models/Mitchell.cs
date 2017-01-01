using System;
using System.Linq;

namespace LanfeustBridge.Models
{
    public class Mitchell : IMovement
    {
        public MovementDescription MovementDescription { get; } = new MovementDescription
        {
            Id = typeof(Mitchell).Name.ToLower(),
            Name = "Mitchell",
            Description = "Standard Mitchell : NS fixed, EW move to next table, boards to previous",
            MinTables = 3,
            MinRounds = 2
        };

        public Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound)
        {
            if (nbTables < 3)
                throw new NotSupportedException("At least 3 tables needed for Mitchell");
            if (nbRounds > nbTables - (nbTables + 1) % 2)
                throw new NotSupportedException($"At most {nbTables - (nbTables + 1) % 2} rounds possible for a {nbTables} Mitchell");
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

        public MovementValidation Validate(int nbTables, int nbRounds)
        {
            return new MovementValidation { IsValid = nbTables >= 3 && nbRounds >= 2 && nbRounds <= nbTables - (nbTables + 1) % 2 };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public interface IMovement
    {
        Position[][] GetPositions(int nbTables, int nbRounds, int nbDealsPerRound);
        Deal[] CreateDeals(int nbTables, int nbRounds, int nbDealsPerRound);
    }
}

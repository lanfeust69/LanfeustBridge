using System.Collections.Generic;
using System.Linq;

namespace LanfeustBridge.Tests
{
    using LanfeustBridge.Models;

    internal class PositionComparer : IEqualityComparer<Position>
    {
        public bool Equals(Position x, Position y)
        {
            if (x.Table != y.Table)
                return false;
            if (x.Deals.Length != y.Deals.Length || x.Deals.Zip(y.Deals, (a, b) => a != b).Any(b => b))
                return false;
            return x.East == y.East && x.North == y.North && x.West == y.West && x.South == y.South;
        }

        public int GetHashCode(Position p)
        {
            return p.Table; // not really used, no matter if often collides
        }
    }
}

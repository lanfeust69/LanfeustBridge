using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Position
    {
        public int Table { get; set; }
        public int[] Deals { get; set; }
        public int West { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int South { get; set; }
    }
}

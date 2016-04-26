using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Contract
    {
        public string Declarer { get; set; }
        public int Level { get; set; }
        public Suit Suit { get; set; }
        public bool Doubled { get; set; }
        public bool Redoubled { get; set; }
    }
}

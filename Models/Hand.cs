using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Hand
    {
        public string[] Spades { get; set; }
        public string[] Hearts { get; set; }
        public string[] Diamonds { get; set; }
        public string[] Clubs { get; set; }
    }
}

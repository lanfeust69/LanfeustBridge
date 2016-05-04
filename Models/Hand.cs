using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Hand
    {
        public Hand()
        {
            Spades = new string[0];
            Hearts = new string[0];
            Diamonds = new string[0];
            Clubs = new string[0];
        }

        public string[] Spades { get; set; }
        public string[] Hearts { get; set; }
        public string[] Diamonds { get; set; }
        public string[] Clubs { get; set; }
    }
}

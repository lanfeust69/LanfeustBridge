using System;

namespace LanfeustBridge.Models
{
    public class Hand
    {
        public Hand()
        {
            Spades = Array.Empty<string>();
            Hearts = Array.Empty<string>();
            Diamonds = Array.Empty<string>();
            Clubs = Array.Empty<string>();
        }

        public string[] Spades { get; set; }

        public string[] Hearts { get; set; }

        public string[] Diamonds { get; set; }

        public string[] Clubs { get; set; }
    }
}

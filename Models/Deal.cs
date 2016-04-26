using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LanfeustBridge.Models
{
    public class Deal
    {
        public int Id { get; set; }
        public string Dealer { get; set; }
        public string Vulnerability { get; set; }
        public Hands Hands { get; set; }
        public List<Score> Scores { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    public class Dive 
    {
        public int Id { get; set; }
        
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; } //1-N
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; } //1-N
        public string DiveGroup { get; set; }
        public double? Judge1 { get; set; }
        public double? Judge2 { get; set; }
        public double? Judge3 { get; set; }
        public double? Score { get; set; }
    }
}

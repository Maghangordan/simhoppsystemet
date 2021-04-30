using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    public class Dive 
    {
        public Competitor Competitor { get; set; }
        public int CompetitionId { get; set; }
        public int CompetitorId { get; set; }
        public int DiveGroup { get; set; }
        public double PointsA { get; set; }
        public double PointsB { get; set; }
        public double PointsC { get; set; }
        public double FinalScore { get; set; }

    }
}

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
        public int DiveGroup { get; set; }
        public double PointsA { get; set; }
        public double PointsB { get; set; }
        public double PointsC { get; set; }
        public double FinalScore { get; set; }
        
        
    }
}

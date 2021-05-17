using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]  // Shows only 2 decimals
        [Range(0.0, 10.0)] // Sets range between 0 - 10
        public double? Judge1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]  // Shows only 2 decimals
        [Range(0.0, 10.0)] // Sets range between 0 - 10
        public double? Judge2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]  // Shows only 2 decimals
        [Range(0.0, 10.0)] // Sets range between 0 - 10
        public double? Judge3 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]  // Shows only 2 decimals
        public double? Score { get; set; }
    }
}

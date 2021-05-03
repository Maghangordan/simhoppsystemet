using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    public class Competition
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public List<Dive> Dives { get; set; } //1-N
        public ICollection<CompetitionCompetitor> CompetitionCompetitor { get; set; }

        public Competition()
        {
            
        }
    }
    public class CompetitionCompetitor
    {
        public int CompetitionCompetitorId { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int CompetitorId { get; set; }
        public Competitor Competitor { get; set; }
    }
    
}

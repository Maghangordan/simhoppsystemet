using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simhoppsystemet.Models
{
    public class Competitor
    {
        public int Id { get; set; }
        public int Age { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Gender { get; set; } //String due to political reasons
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Organization { get; set; }

        public List<Dive> Dives = new List<Dive>();
        
    }
}

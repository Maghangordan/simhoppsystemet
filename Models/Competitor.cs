﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    

    public class Competitor
    {
        public int Id { get; set; }
        [Range(0,120, ErrorMessage ="Ange en ålder mellan 0 och 120.")]
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
        public List<Dive> Dives { get; set; } //1-N
        
        public ICollection<CompetitionCompetitor> CompetitionCompetitor { get; set; }
        

    }
}

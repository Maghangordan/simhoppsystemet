using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    public class DiveGroup
    {
        public int Id { get; set; }
        public string Dive { get; set; }
        public float? Difficulty { get; set; }
    }
}

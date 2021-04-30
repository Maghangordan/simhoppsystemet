using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simhoppsystemet.Models
{
    public class Competitor
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; } //String due to political reasons
        public string Organization { get; set; }

        public List<Dive> Dives = new List<Dive>();
        
    }
}

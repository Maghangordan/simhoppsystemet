using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using simhoppsystemet.Models;

namespace simhoppsystemet.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<simhoppsystemet.Models.Competition> Competition { get; set; }
        public DbSet<simhoppsystemet.Models.Competitor> Competitor { get; set; }
        public DbSet<simhoppsystemet.Models.Dive> Dive { get; set; }
    }
}

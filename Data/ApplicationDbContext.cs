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
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CompetitionCompetitor>(b =>
        //    {
        //        b.HasIndex(e => new { e.CompetitionId, e.CompetitorId }).IsUnique(); //The combination between the CompetitionId and CompetitorId in CompetitionCompetitor is unique
        //    });
        //}
        public DbSet<simhoppsystemet.Models.Competition> Competition { get; set; }
        public DbSet<simhoppsystemet.Models.Competitor> Competitor { get; set; }
        public DbSet<simhoppsystemet.Models.Dive> Dive { get; set; }
        public DbSet<simhoppsystemet.Models.CompetitionCompetitor> CompetitionCompetitor { get; set; }
        public DbSet<simhoppsystemet.Models.DiveGroup> DiveGroup { get; set; }
    }
}

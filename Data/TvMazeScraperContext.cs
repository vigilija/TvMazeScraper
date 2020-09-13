using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Entities;

namespace TvMazeScraper.Data
{
    public class TvMazeScraperContext : DbContext
    {
        public TvMazeScraperContext (DbContextOptions<TvMazeScraperContext> options)
            : base(options)
        {
        }

        public DbSet<Show> Show { get; set; }
        public DbSet<Actor> Actor { get; set; }
        public DbSet<ShowActors> ShowActors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // many-to-many relationship between shows and actors
            modelBuilder.Entity<ShowActors>().HasKey(c => new { c.ActorId, c.ShowId });
            modelBuilder.Entity<ShowActors>().HasOne(a => a.Actor).WithMany(c => c.Cast).HasForeignKey(a => a.ActorId);
            modelBuilder.Entity<ShowActors>().HasOne(sh => sh.Show).WithMany(c => c.Cast).HasForeignKey(sh => sh.ShowId);
        }
    }
}
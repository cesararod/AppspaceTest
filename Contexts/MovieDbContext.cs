using CRApiSolution.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CRApiSolution.Contexts
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Cinema> Cinema { get; set; }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<City> Citie { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<MovieGenre> MovieGenre { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Session> Session { get; set; }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });
        }
    }
}

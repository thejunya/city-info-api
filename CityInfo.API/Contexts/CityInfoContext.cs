using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Contexts;

public sealed class CityInfoContext : DbContext
{
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Attraction> Attractions { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public CityInfoContext(DbContextOptions<CityInfoContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City()
            {
                Id = 1,
                Name = "New York City",
                Description = "The one with that big park."
            },
            new City()
            {
                Id = 2,
                Name = "Antwerp",
                Description = "The one with the cathedral that was never really finished."
            },
            new City()
            {
                Id = 3,
                Name = "Paris",
                Description = "The one with that big tower."
            });

        modelBuilder.Entity<Attraction>().HasData(
            new Attraction()
            {
                Id = 1,
                Name = "Central Park",
                CityId = 1,
                Description = "The most visited urban park in the United States."
            },
            new Attraction()
            {
                Id = 2,
                Name = "Empire State Building",
                CityId = 1,
                Description = "A 102-story skyscraper located in Midtown Manhattan."
            },
            new Attraction()
            {
                Id = 3,
                Name = "Cathedral",
                CityId = 2,
                Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
            },
            new Attraction()
            {
                Id = 4,
                Name = "Antwerp Central Station",
                CityId = 2,
                Description = "The the finest example of railway architecture in Belgium."
            },
            new Attraction()
            {
                Id = 5,
                Name = "Eiffel Tower",
                CityId = 3,
                Description = "Like... You know."
            },
            new Attraction()
            {
                Id = 6,
                Name = "The Louvre",
                CityId = 3,
                Description = "The world's largest museum."
            }
        );

        modelBuilder.Entity<User>().HasData(
            new User()
            {
                Id = 1,
                Login = "Junya",
                Password = "StrongPassword",
                IsAdmin = true,
            },
            new User()
            {
                Id = 2,
                Login = "Unknown",
                Password = "UnknownPassword",
                IsAdmin = true,
            },
            new User()
            {
                Id = 3,
                Login = "Magnus Carlsen",
                Password = "ChessPlayerPassword",
                IsAdmin = true,
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}
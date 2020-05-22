using DogsIRL_API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CollectedPetCard>().HasKey(collect => new { collect.PetCardID, collect.Username });

            modelBuilder.Entity<PetCard>().HasData(
               new PetCard
                {
                    ID = 1,
                    Name = "Tucker",
                    ImageURL = "https://dogsirl.blob.core.windows.net/dogs/Tucker.png",
                    Sex = "Male",
                    Owner = "andrewbc",
                    AgeYears = 2,
                    Birthday = DateTime.Parse("07/10/2018"),
                    DateCreated = DateTime.Now,
                    DateCollected = DateTime.Now,
                    GoodDog = 8,
                    Floofiness = 1,
                    Energy = 8,
                    Snuggles = 8,
                    Appetite = 8,
                    Bravery = 9
                }
               );
        }

        public DbSet<PetCard> PetCards { get; set; }
        public DbSet<CollectedPetCard> CollectedPetCards { get; set; }

    }
}

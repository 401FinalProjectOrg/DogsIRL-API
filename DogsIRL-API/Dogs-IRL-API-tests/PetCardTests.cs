using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DogsIRL_API.Controllers;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Services;
using DogsIRL_API.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dogs_IRL_API_tests
{
    public class PetCardTests
    {
        [Fact]
        public async Task CanCreatePetCard()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestCanCreatePetCard")
                .Options;
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                var testPetCardsService = new PetCardsService(context);
                var testPetCard = new PetCard()
                {
                    Name = "TestDog",
                    ImageURL = String.Empty,
                    Sex = "Female",
                    Owner = "Test",
                    AgeYears = 0,
                    Birthday = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateCollected = DateTime.Now,
                    GoodDog = 1,
                    Floofiness = 1,
                    Energy = 1,
                    Snuggles = 1,
                    Appetite = 1,
                    Bravery = 1,
                    Collections = 1
                };
                var result = await testPetCardsService.CreatePetCard(testPetCard);
                Assert.Equal(testPetCard, result);
            }
                
        }
    }
}

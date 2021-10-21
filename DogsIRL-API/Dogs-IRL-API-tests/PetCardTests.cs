using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DogsIRL_API.Controllers;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Services;
using DogsIRL_API.Data;
using Microsoft.EntityFrameworkCore;

namespace Dogs_IRL_API_tests
{
    public class PetCardTests
    {
        [Fact]
        public void CanCreatePetCard()
        {
            DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestCanCreatePetCard")
                .Options;
            using (ApplicationDbContext context = new ApplicationDbContext(options))
            {
                var TestPetCardsService = new PetCardsService(context);
            }
                
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CollectionController : ControllerBase
    {
        private readonly IPetCardsManager _petCardsService;

        public CollectionController(IPetCardsManager petCardsService)
        {
            _petCardsService = petCardsService;
        }

        [HttpPost]
        public async Task AddPetCardToUserCollection(CollectInput input)
        {
            var identity = User.Claims;
            if (identity != null)
            {
                string tokenUsername = identity.FirstOrDefault(x => x.Type == "UserName").Value;
                if (tokenUsername == input.Username)
                {
                    PetCard petCard = await _petCardsService.GetPetCardById(input.PetCardID);
                    await _petCardsService.AddPetCardToUserCollection(petCard, input.Username);
                }
            }
        }

        [HttpGet("{username}")]
        public async Task<List<CollectedPetCard>> GetAllCollectedPetCardsForUser(string username)
        {
            List<CollectedPetCard> list = new List<CollectedPetCard>();

            var identity = User.Claims;
            if (identity != null)
            {
                string tokenUsername = identity.FirstOrDefault(x => x.Type == "UserName").Value;
                if (tokenUsername == username)
                {
                    list = await _petCardsService.GetAllCollectedPetCardsForUser(username);
                    return list;
                }
            }
            return list;
            
        }

    }
}

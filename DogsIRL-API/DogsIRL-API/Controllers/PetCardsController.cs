using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PetCardsController : ControllerBase
    {
        private readonly IPetCardsManager _petCardsService;
        private IConfiguration _configuration;
        private readonly ILogger<PetCardsController> _logger;


        public PetCardsController(ILogger<PetCardsController> logger, IPetCardsManager petCardsService, IConfiguration configuration)
        {
            _logger = logger;
            _petCardsService = petCardsService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<List<PetCard>> GetAllPetCards()
        {
            return await _petCardsService.GetAllPetCards();
        }

        [HttpGet("{petCardId}")]
        public async Task<PetCard> GetPetCardById(int petCardId)
        {
            return await _petCardsService.GetPetCardById(petCardId);
        }

        [HttpGet("user/{username}")]
        public async Task<List<PetCard>> GetAllPetCardsForOwnerByUserName(string username)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string tokenUsername = identity.FindFirst("username").Value;
                if(tokenUsername == username)
                {
                    return await _petCardsService.GetPetCardsForOwnerByUsername(username);
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult<PetCard>> CreatePetCard(PetCard petcard)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string tokenUsername = identity.FindFirst("username").Value;
                if (tokenUsername == petcard.Owner)
                {
                    await _petCardsService.CreatePetCard(petcard);
                    return CreatedAtAction("CreatePetCard", new { id = petcard.ID }, petcard);
                }
            }
            return null;
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<PetCard>> DeletePetCard(int ID, PetCard petCard)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string tokenUsername = identity.FindFirst("username").Value;
                if (tokenUsername == petCard.Owner)
                {
                    if (ID != petCard.ID)
                    {
                        return BadRequest();
                    }
                    await _petCardsService.DeletePetCard(petCard);
                    return NoContent();
                }
            }
            return null;
            
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePetCard(int ID, PetCard petCard)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                string tokenUsername = identity.FindFirst("username").Value;
                if (tokenUsername == petCard.Owner)
                {
                    if (ID != petCard.ID)
                    {
                        return BadRequest();
                    }
                    await _petCardsService.UpdatePetCard(petCard);
                    return NoContent();
                }
            }
            return null;
        }

    }
}

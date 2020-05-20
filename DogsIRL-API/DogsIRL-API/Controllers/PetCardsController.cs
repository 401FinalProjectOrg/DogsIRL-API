using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PetCardsController : ControllerBase
    {
        private readonly IPetCardsManager _petCardsService;

        private readonly ILogger<PetCardsController> _logger;

        public PetCardsController(ILogger<PetCardsController> logger, IPetCardsManager petCardsService)
        {
            _logger = logger;
            _petCardsService = petCardsService;
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

        [HttpGet("user/{userName}")]
        public async Task<List<PetCard>> GetAllPetCardsForOwnerByUserName(string userName)
        {
            return await _petCardsService.GetPetCardsForOwnerByUsername(userName);
        }

        [HttpPost]
        public async Task<ActionResult<PetCard>> CreatePetCard(PetCard petcard)
        {
            await _petCardsService.CreatePetCard(petcard);
            return CreatedAtAction("CreatePetCard", new { id = petcard.ID }, petcard);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<PetCard>> DeletePetCard(int ID, PetCard petCard)
        {
            if (ID != petCard.ID)
            {
                return BadRequest();
            }
            await _petCardsService.DeletePetCard(petCard);
            return NoContent();
        }

        //Shouldnt this take in ID?
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePetCard(int ID, PetCard petCard)
        {
            if(ID != petCard.ID)
            {
                return BadRequest();
            }
            await _petCardsService.UpdatePetCard(petCard);
            return NoContent();
        }
    }
}

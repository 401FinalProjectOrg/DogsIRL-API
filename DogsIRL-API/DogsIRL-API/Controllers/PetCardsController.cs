using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile file)
        {
            var filePath = Path.GetTempFileName();
            // stream io to save to file location
            var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);
            string URL = await UploadToBlob(stream);
            return URL;
        }


        private async Task<string> UploadToBlob(Stream stream)
        {
            string storageAccountName = _configuration["Storage-Account-Name"];
            string storageAccountKey = _configuration["BlobKey"];
            var account = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName={storageAccountName};AccountKey={storageAccountKey};EndpointSuffix=core.windows.net");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("dogsirl");
            await container.CreateIfNotExistsAsync();
            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.png");
            await blockBlob.UploadFromStreamAsync(stream);
            string URL = blockBlob.Uri.OriginalString;
            return URL;
        }
    }
}

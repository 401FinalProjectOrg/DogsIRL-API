using DogsIRL_API.Models;
using DogsIRL_API.Models.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionManager _interactionService;

        public InteractionController(IInteractionManager interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpGet("random")]
        public async Task<ActionResult<Interaction>> GetRandomInteraction()
        {

            Interaction randomResult = await _interactionService.GetRandomInteraction();
            return Ok(randomResult);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Interaction>> GetInteractionById(int id)
        {
            Interaction result = await _interactionService.GetInteractionById(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<Interaction>>> GetAllInteractions()
        {
            IEnumerable<Interaction> results = await _interactionService.GetAllInteractions();
            return Ok(results);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Interaction>> AddInteraction(Interaction interaction)
        {
            Interaction added = await _interactionService.AddInteraction(interaction);
            return Ok(added);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Interaction>> EditInteraction(int id, Interaction interaction)
        {
            if(id != interaction.Id)
            {
                return BadRequest();
            }

            await _interactionService.EditInteraction(interaction);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<Interaction>> DeleteInteraction(int id, Interaction interaction)
        {
            if (id != interaction.Id)
            {
                return BadRequest();
            }

            await _interactionService.DeleteInteraction(interaction);

            return Ok();
        }
    }
}

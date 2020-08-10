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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionManager _interactionService;

        public InteractionController(IInteractionManager interactionService)
        {
            _interactionService = interactionService;
        }

        [HttpGet("random")]
        public ActionResult<Interaction> GetRandomInteraction()
        {

            Interaction randomResult = _interactionService.GetRandomInteraction();
            return Ok(randomResult);
        }

        [HttpGet("{id}")]
        public ActionResult<Interaction> GetInteractionById(int id)
        {
            Interaction result = _interactionService.GetInteractionById(id);
            return Ok(result);
        }

        [HttpGet]
        public ActionResult<List<Interaction>> GetAllInteractions()
        {
            List<Interaction> results = _interactionService.GetAllInteractions().ToList();
            return Ok(results);
        }

        [HttpPost]
        public ActionResult<Interaction> AddInteraction(Interaction interaction)
        {
            Interaction added = _interactionService.AddInteraction(interaction);
            return Ok(added);
        }

        [HttpPut("{id}")]
        public ActionResult<Interaction> EditInteraction(int id, Interaction interaction)
        {
            if(id != interaction.Id)
            {
                return BadRequest();
            }
            Interaction result = _interactionService.EditInteraction(interaction);
            return Ok(result);
        }
    }
}

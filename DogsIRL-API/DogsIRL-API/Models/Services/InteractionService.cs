using DogsIRL_API.Data;
using DogsIRL_API.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models.Services
{
    public class InteractionService : IInteractionManager
    {
        private readonly ApplicationDbContext _context;
        public InteractionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Interaction> AddInteraction(Interaction interaction)
        {
            _context.Add(interaction);
            await _context.SaveChangesAsync();
            return interaction;
        }

        public Interaction DeleteInteraction(Interaction interaction)
        {
            throw new NotImplementedException();
        }

        public Interaction EditInteraction(Interaction interaction)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Interaction> GetAllInteractions()
        {
            throw new NotImplementedException();
        }

        public Interaction GetInteractionById(int Id)
        {
            throw new NotImplementedException();
        }

        public Interaction GetRandomInteraction()
        {
            throw new NotImplementedException();
        }
    }
}

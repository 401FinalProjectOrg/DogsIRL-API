using DogsIRL_API.Data;
using DogsIRL_API.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteInteraction(Interaction interaction)
        {
            _context.Remove(interaction);
            await _context.SaveChangesAsync();
        }

        public async Task EditInteraction(Interaction interaction)
        {
            _context.Update(interaction);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Interaction>> GetAllInteractions()
        {
            IEnumerable<Interaction> allInteractions = await _context.Interactions.ToListAsync();
            return allInteractions;
        }

        public async Task<Interaction> GetInteractionById(int Id)
        {
            return await _context.Interactions.FindAsync(Id);
        }

        public async Task<Interaction> GetRandomInteraction()
        {
            var random = new Random();
            List<Interaction> allInteractions = await _context.Interactions.ToListAsync();
            Interaction randomInteraction = allInteractions[random.Next(allInteractions.Count)];
            return randomInteraction;
        }
    }
}

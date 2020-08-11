using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models.Interfaces
{
    public interface IInteractionManager
    {
        public Task<Interaction> GetRandomInteraction();
        public Task<Interaction> GetInteractionById(int Id);
        public Task<IEnumerable<Interaction>> GetAllInteractions();
        public Task<Interaction> AddInteraction(Interaction interaction);
        public Task EditInteraction(Interaction interaction);
        public Task DeleteInteraction(Interaction interaction);
        
    }
}

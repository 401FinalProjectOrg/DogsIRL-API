using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models.Interfaces
{
    public interface IInteractionManager
    {
        public Interaction GetRandomInteraction();
        public Interaction GetInteractionById(int Id);
        public Interaction AddInteraction(Interaction interaction);
        public Interaction EditInteraction(Interaction interaction);
        public Interaction DeleteInteraction(Interaction interaction);
        
    }
}

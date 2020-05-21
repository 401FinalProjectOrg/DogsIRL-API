using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models.Interfaces
{
    public interface IPetCardsManager
    {
        // Create
        public Task<PetCard> CreatePetCard(PetCard petCard);
        public Task<CollectedPetCard> AddPetCardToUserCollection(PetCard petCard, string username);

        // Read
        public Task<PetCard> GetPetCardById(int petCardId);
        public Task<List<PetCard>> GetPetCardsForOwnerByUsername(string ownerUserName);
        public Task<List<PetCard>> GetAllPetCards();
        public Task<List<CollectedPetCard>> GetAllCollectedPetCardsForUser(string username);

        // Update
        public Task<PetCard> UpdatePetCard(PetCard petCard);

        // Delete
        public Task<PetCard> DeletePetCard(PetCard petCard);


    }
}
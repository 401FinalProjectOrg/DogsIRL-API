using DogsIRL_API.Data;
using DogsIRL_API.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models.Services
{
    public class PetCardsService : IPetCardsManager
    {
        private readonly ApplicationDbContext _petCardsContext;

        public PetCardsService(ApplicationDbContext petCardsContext)
        {
            _petCardsContext = petCardsContext;
        }
        public async Task<PetCard> CreatePetCard(PetCard petCard)
        {
            _petCardsContext.Add(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }

        public async Task<PetCard> DeletePetCard(PetCard petCard)
        {
            _petCardsContext.Remove(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }

        public async Task<List<PetCard>> GetAllPetCards()
        {
            List<PetCard> allPetCards = await _petCardsContext.PetCards.ToListAsync();
            return allPetCards;
        }

        public async Task<PetCard> GetPetCardById(int petCardId)
        {
            PetCard foundPetCard = await _petCardsContext.PetCards.FindAsync(petCardId);
            return foundPetCard;
        }

        public async Task<List<PetCard>> GetPetCardsForOwnerByUsername(string ownerUsername)
        {
            List<PetCard> userPetCards = await _petCardsContext.PetCards.Where(petCard => petCard.Owner == ownerUsername).ToListAsync();
            return userPetCards;
        }

        public async Task<PetCard> UpdatePetCard(PetCard petCard)
        {
            _petCardsContext.PetCards.Update(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }
    }
}

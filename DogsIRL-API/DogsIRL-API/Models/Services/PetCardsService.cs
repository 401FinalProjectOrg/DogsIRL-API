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

        /// <summary>
        /// Adds the given pet card to the database, and saves the changes.
        /// </summary>
        /// <param name="petCard">The new petcard to add to the database</param>
        /// <returns>The new petcard, after it is added</returns>
        public async Task<PetCard> CreatePetCard(PetCard petCard)
        {
            _petCardsContext.Add(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }

        /// <summary>
        /// Deletes the given pet card from the database, and saves the changes
        /// </summary>
        /// <param name="petCard">The petcard to delete</param>
        /// <returns>The deleted pet card, after it is deleted</returns>
        public async Task<PetCard> DeletePetCard(PetCard petCard)
        {
            _petCardsContext.Remove(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }

        /// <summary>
        /// Gets a list of all the pet cards in the database.
        /// </summary>
        /// <returns>The list of all the pet cards in the database</returns>
        public async Task<List<PetCard>> GetAllPetCards()
        {
            List<PetCard> allPetCards = await _petCardsContext.PetCards.ToListAsync();
            return allPetCards;
        }

        /// <summary>
        /// Gets the pet card indicated by the pet card ID
        /// </summary>
        /// <param name="petCardId">The ID of the pet card to get</param>
        /// <returns>The pet card to get, once it's retrieved</returns>
        public async Task<PetCard> GetPetCardById(int petCardId)
        {
            PetCard foundPetCard = await _petCardsContext.PetCards.FindAsync(petCardId);
            return foundPetCard;
        }

        /// <summary>
        /// Gets a list of all the pet cards that are marked as owned by the given username.
        /// </summary>
        /// <param name="ownerUsername">The username of the owner for whom we will retrieve cards</param>
        /// <returns>The list of pet cards owned, once they are retrieved</returns>
        public async Task<List<PetCard>> GetPetCardsForOwnerByUsername(string ownerUsername)
        {
            List<PetCard> userPetCards = await _petCardsContext.PetCards.Where(petCard => petCard.Owner == ownerUsername).ToListAsync();
            return userPetCards;
        }

        /// <summary>
        /// Updates the pet card in the database that matches the ID of the inputted pet card with the inputted pet card
        /// </summary>
        /// <param name="petCard">The updated pet card</param>
        /// <returns>The updated pet card, once it is updated</returns>
        public async Task<PetCard> UpdatePetCard(PetCard petCard)
        {
            _petCardsContext.PetCards.Update(petCard);
            await _petCardsContext.SaveChangesAsync();
            return petCard;
        }


        // Collect Petcard in park
        // TODO: Move to separate service

        /// <summary>
        /// Adds the inputted pet card to the collection of the given user via the join table if the user has not already collected it
        /// </summary>
        /// <param name="petCard">The petcard to add to the collection</param>
        /// <param name="username">The username of the user with the collection we are adding to</param>
        /// <returns>The collected pet card entry, after completion. Returns null if the card is not added.</returns>
        public async Task<CollectedPetCard> AddPetCardToUserCollection(PetCard petCard, string username)
        {
            bool alreadyExist = await CheckCollectedPetCardExist(petCard.ID, username); // Whether the card already exists in username's collection

            if (alreadyExist)
            {
                return null;
            }
            else
            {
                CollectedPetCard newCPC = new CollectedPetCard();
                newCPC.PetCardID = petCard.ID;
                newCPC.Username = username;
                _petCardsContext.CollectedPetCards.Add(newCPC);
                await _petCardsContext.SaveChangesAsync();
                return newCPC;
            }
        }

        /// <summary>
        /// Tries to get the pet card with the given pet card Id from the collection of username, and returns whether it is successful
        /// </summary>
        /// <param name="petcardId">The Id of the pet card to search for in the collection</param>
        /// <param name="username">The name of the user with the collection</param>
        /// <returns>Whether the pet card was found in username's collection. Returns true if a result is found, false otherwise</returns>
        public async Task<bool> CheckCollectedPetCardExist(int petcardId, string username)
        {
            var result = await _petCardsContext.CollectedPetCards.FindAsync(petcardId, username);

            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the list of all the pet cards in the collection of username
        /// </summary>
        /// <param name="username">The name of the user with the collection to retrieve</param>
        /// <returns>The list of all the collected pet cards, once retrieved</returns>
        public async Task<List<CollectedPetCard>> GetAllCollectedPetCardsForUser(string username)
        {
            var query = _petCardsContext.CollectedPetCards
                .Where(cpc => cpc != null && cpc.Username == username);

            List<CollectedPetCard> list = new List<CollectedPetCard>();

            if(query.ToList() != null)
            {
                list = await query.Include(cpc => cpc.PetCard).ToListAsync();
            };
                
            return list;
        }

    }
}

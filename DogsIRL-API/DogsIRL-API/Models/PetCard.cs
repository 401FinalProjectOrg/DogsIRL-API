using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogsIRL_API.Models
{
    public class PetCard
    {
        public int ID;
        public string Name;
        public string ImageURL;
        public string Sex;
        public string Owner;
        public int AgeYears;
        public DateTime Birthday;
        public DateTime DateCreated;
        public DateTime DateCollected;
        public sbyte GoodDog;
        public sbyte Floofiness;
        public sbyte Energy;
        public sbyte Snuggles;
        public sbyte Appetite;
        public sbyte Bravery;
    }
}

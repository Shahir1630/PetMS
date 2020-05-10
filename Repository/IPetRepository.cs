using PetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Repository
{
    public interface IPetRepository
    {
        void Create(Pet pet);
        void Edit(Pet pet);
        Pet GetSinglePet(int id);
        void Delete(Pet pet);
        IEnumerable<Pet> GetAllPets();
        bool VerifyName(string name);
        IEnumerable<Pet> SearchPets(string search);
        IEnumerable<Pet> GetPetByUserId(string userId);
    }
}

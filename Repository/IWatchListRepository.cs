using PetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Repository
{
    public interface IWatchListRepository
    {
        void Create(WatchList watchList);
        IEnumerable<WatchList> GetWatchLists(string userId);
        WatchList GetWatchListById(int Id);
        void Remove(WatchList watchList);

        bool CheckIfAllreadyExits(string userId, int petId);

        IEnumerable<WatchList> GetWatchListFromPetId(int petId);
    }
}

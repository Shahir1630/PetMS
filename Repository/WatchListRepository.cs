using Microsoft.EntityFrameworkCore;
using PetDemo.Data;
using PetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Repository
{
    public class WatchListRepository : IWatchListRepository
    {
        private ApplicationDbContext _context;

        public WatchListRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CheckIfAllreadyExits(string userId, int petId)
        {
            return _context.WatchLists.Any(w => w.UserId == userId && w.PetId == petId);
        }

        public void Create(WatchList watchList)
        {
            _context.WatchLists.Add(watchList);
            _context.SaveChanges();
        }

        public WatchList GetWatchListById(int Id)
        {
            return _context.WatchLists.FirstOrDefault(w => w.Id == Id);
        }

        public IEnumerable<WatchList> GetWatchListFromPetId(int petId)
        {
            return _context.WatchLists.Where(w => w.PetId == petId).ToList();
        }

        public IEnumerable<WatchList> GetWatchLists(string userId)
        {
            return _context.WatchLists
                .Include(w=>w.Pet)
                .Where(w => w.UserId == userId)
                .ToList();
        }

        public void Remove(WatchList watchList)
        {
            _context.WatchLists.Remove(watchList);
            _context.SaveChanges();
        }
    }
}

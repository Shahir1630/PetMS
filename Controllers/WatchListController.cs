using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using PetDemo.Models;
using PetDemo.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetDemo.Controllers
{
    [Authorize]
    public class WatchListController : Controller
    {
        private IWatchListRepository _watchListRepo;
        private UserManager<ApplicationUser> _userManager;
        private IToastNotification _toastNotification;

        public WatchListController(UserManager<ApplicationUser> userManager,
            IWatchListRepository watchListRepo,IToastNotification toastNotification)
        {
            _watchListRepo = watchListRepo;
            _userManager = userManager;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var watchList = _watchListRepo.GetWatchLists(userId);
            return View(watchList);
        } 
        public IActionResult New(int petId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var watchList = new WatchList
            {
                PetId = petId,
                UserId = userId,
            };

            _watchListRepo.Create(watchList);
            _toastNotification.AddSuccessToastMessage("Pet Has Been Added To WatchList Successfully");

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int id)
        {
            var watchList = _watchListRepo.GetWatchListById(id);
            _watchListRepo.Remove(watchList);

            _toastNotification.AddErrorToastMessage("Pet Has Been Removed To WatchList Successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}

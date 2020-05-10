using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using PetDemo.Data;
using PetDemo.Models;
using PetDemo.Repository;

namespace PetDemo.Controllers
{
    public class PetController : BaseController<Pet>
    {
        private IPetRepository _petRepo;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationRepository _notificationRepo;

        public PetController(IPetRepository petRepository,IToastNotification toastNotification,
                                UserManager<ApplicationUser> userManager, INotificationRepository notificationRepo)
        {
            _petRepo = petRepository;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _notificationRepo = notificationRepo;
        }
        public IActionResult Index(string search, int page =1)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var foundPets = _petRepo.SearchPets(search);
                return View(foundPets);
            }
            var pets = _petRepo.GetAllPets();
            //return View(pets);

            //pagination
            var enumerable = pets;
            List<Pet> asList = enumerable.ToList();//convert IEnumerable to list


            var paginatedResult = PaginatedResult(asList, page, 1);
            return View(paginatedResult);
        } 

        public IActionResult MyPets()
        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            var pets = _petRepo.GetPetByUserId(userId);
            return View(pets);
        }
        public IActionResult Details(int id)
        {
            var pet = _petRepo.GetSinglePet(id);
            return View(pet);
        }

        public IActionResult AddNew()
        {
            ViewBag.IsEditMode = "false";
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddNew(Pet pet, string IsEditMode)
        {
            var userId = _userManager.GetUserId(this.HttpContext.User);
            pet.UserId = userId;
            if (!ModelState.IsValid)
            {
                ViewBag.IsEditMood = IsEditMode;
                return View(pet);
            }
            else
            {
                if (IsEditMode.Equals("false"))
                {
                    _petRepo.Create(pet);
                    _toastNotification.AddSuccessToastMessage("Your Pet Is Created Successfully");
                    //_clientNotification.AddSweetNotification("successfully", "Your Pet Is Created.", NotificationType.success);
                }
                else
                {
                    _petRepo.Edit(pet);
                    _toastNotification.AddSuccessToastMessage("Your Pet Is Eidited Successfully");
                }

                return RedirectToAction("Index");
            }
        }
        public IActionResult Delete(int Id)
        {
            var pet = _petRepo.GetSinglePet(Id);
            _petRepo.Delete(pet);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int Id)
        {
            var loggedInUserId = _userManager.GetUserId(HttpContext.User);

            ViewBag.IsEditMode = "true";
            var pet = _petRepo.GetSinglePet(Id);

            if(pet.UserId != loggedInUserId)
            {
                return Content("You are not authorized for this action");
            }

            _petRepo.Delete(pet);       //for remove same name problem and it delete the pat if some try to eddit other pat
            _toastNotification.AddWarningToastMessage("It Will Change the Pet Information!");

            return View("AddNew",pet);
        }

        public IActionResult ToggleSelling(int id)
        {
            var pet = _petRepo.GetSinglePet(id);
            pet.IsSelling = !pet.IsSelling;
            _petRepo.Edit(pet);

            //Selling notification

            var username = _userManager.GetUserName(HttpContext.User);
            var status = "";
            if (pet.IsSelling)
                status = "Selling";
            else
                status = "Not Selling";
            var notificaton = new Notification
            {
                Text = $"The {username} is {status} their pet." 
            };

            _notificationRepo.Create(notificaton, pet.Id);

            return RedirectToAction(nameof(Details), new { id = id });      //new { id(Details action perametter have to same) = id }
        }

        public IActionResult VerifyName(string Name)
        {
            if(_petRepo.VerifyName(Name))
            {
                return Json($"This Name {Name} Is Already Exists. Please Try Another Name");
            }
            return Json(true);
        }

        public IActionResult AutoCompleteResult(string search)
        {
            return Json(_petRepo.SearchPets(search));
        }
    }
}
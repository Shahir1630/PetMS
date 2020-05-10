using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetDemo.Models;
using PetDemo.Repository;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetDemo.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private INotificationRepository _notificationRepo;
        private UserManager<ApplicationUser> _userManager;

        // GET: /<controller>/

        public NotificationController(INotificationRepository notificationRepo, UserManager<ApplicationUser> userManager)
        {
            _notificationRepo = notificationRepo;
            _userManager = userManager;
        }
        public IActionResult GetNotification()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var notification = _notificationRepo.GetUserNotifications(userId);
            return Ok(new {UserNotification = notification, notification.Count });
        }
        public IActionResult ReadNotification(int notificationId) ///what if id is null add here check notirepo
        {
            if(notificationId != 0)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                _notificationRepo.ReadNotification(notificationId, userId);
                return Ok();
            }
           
            return Ok();
        }
        //public IActionResult Details(int notificationId)
        //{
        //    var notification = _notificationRepo.ReadSingleNotification(notificationId);
        //    return View(notification);
        //}
    }
}

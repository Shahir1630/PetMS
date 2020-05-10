using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PetDemo.Data;
using PetDemo.Infrastructure;
using PetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private ApplicationDbContext _context;
        private IWatchListRepository _watchListRepository;
        private IHubContext<SignalServer> _hubContext;

        public NotificationRepository(ApplicationDbContext context, IWatchListRepository watchListRepository, IHubContext<SignalServer> hubContext)
        {
            _context = context;
            _watchListRepository = watchListRepository;
            _hubContext = hubContext;
        }
        public void Create(Notification notification, int petId)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();

            //Todo: Assign notification to user

            var watchLists = _watchListRepository.GetWatchListFromPetId(petId);
            foreach(var watchList in watchLists)
            {
                var userNotification = new NotificationAppUser();
                userNotification.ApplicationUserId = watchList.UserId;
                userNotification.NotificationId = notification.Id;

                _context.NotificationAppUsers.Add(userNotification);
                _context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("displayNotification", "");
        }

        public List<NotificationAppUser> GetUserNotifications(string userId)
        {
            return _context.NotificationAppUsers.Where(u => u.ApplicationUserId == userId && !u.IsRead)
                                                .Include(n => n.Notification)
                                                .ToList();
        }

        public void ReadNotification(int notificationId, string userId )
        {
            var notification = _context.NotificationAppUsers.FirstOrDefault(n => n.ApplicationUserId == userId 
                                                                               && n.NotificationId==notificationId);
            notification.IsRead = true;
            _context.NotificationAppUsers.Update(notification); ///show null refference need to change
            _context.SaveChanges();
        }

        //to see notification
        //public NotificationAppUser ReadSingleNotification(int notificationId)
        //{
        //    return _context.NotificationAppUsers.FirstOrDefault(n => n.NotificationId == notificationId);
        //}
    }
}

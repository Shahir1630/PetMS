using PetDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Repository
{
    public interface INotificationRepository
    {
        List<NotificationAppUser> GetUserNotifications(string userId);
        void Create(Notification notification, int petId);
        void ReadNotification(int notificationId, string userId);

        //to see notification details on another page
        //NotificationAppUser ReadSingleNotification(int notificationId);
    }
}

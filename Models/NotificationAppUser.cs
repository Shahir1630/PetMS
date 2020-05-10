using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Models
{
    public class NotificationAppUser
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public bool IsRead { get; set; } = false;

    }
}

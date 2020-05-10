using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public IEnumerable<NotificationAppUser> NotificationAppUsers { get; set; }

    }
}

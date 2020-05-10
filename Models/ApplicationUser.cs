using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetDemo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<Pet> Pets { get; set; }
        public IEnumerable<WatchList> WatchLists { get; set; }

        public IEnumerable<NotificationAppUser> NotificationAppUsers { get; set; }
    }
}

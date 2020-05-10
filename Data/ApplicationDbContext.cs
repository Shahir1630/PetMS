using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetDemo.Models;

namespace PetDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationAppUser> NotificationAppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder  modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(p => p.Pets)
                .WithOne(u => u.User)
                .IsRequired();

            modelBuilder.Entity<Pet>()
               .HasMany(w => w.WatchLists)
               .WithOne(p => p.Pet)
               .IsRequired();

            modelBuilder.Entity<ApplicationUser>()
               .HasMany(w => w.WatchLists)
               .WithOne(u => u.User)
               .IsRequired();

            //notification many to many
            modelBuilder.Entity<NotificationAppUser>()
                .HasKey(nau=> new { nau.NotificationId, nau.ApplicationUserId });


            base.OnModelCreating(modelBuilder);
        }
    }
}

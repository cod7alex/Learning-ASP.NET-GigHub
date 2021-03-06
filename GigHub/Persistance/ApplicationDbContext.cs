﻿using GigHub.Core.Models;
using GigHub.Persistance.EntityConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace GigHub.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Gig> Gigs { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Following> Followings { get; set; }

        public DbSet<Notification> Notification { get; set; }

        public DbSet<UserNotification> UserNotification { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations
                .Add(new GigConfiguration())
                .Add(new NotificationConfiguration())
                .Add(new UserNotificationConfiguration())
                .Add(new GenreConfiguration())
                .Add(new UserConfiguration())
                .Add(new AttendanceConfiguration())
                .Add(new FollowingConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
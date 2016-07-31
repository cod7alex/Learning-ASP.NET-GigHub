using GigHub.Core.Models;
using System.Data.Entity;

namespace GigHub.Persistance
{
    public interface IApplicationDbContext
    {
        DbSet<Attendance> Attendances { get; set; }

        DbSet<Following> Followings { get; set; }

        DbSet<Genre> Genres { get; set; }

        DbSet<Gig> Gigs { get; set; }

        DbSet<Notification> Notification { get; set; }

        DbSet<UserNotification> UserNotification { get; set; }
    }
}
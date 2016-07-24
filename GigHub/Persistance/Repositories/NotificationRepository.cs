using GigHub.Core.Models;
using GigHub.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Persistance.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Notification> GetNotificationsWithArtist(string userId)
        {
            return _context.UserNotification
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(un => un.Gig.Artist);
        }

        public IEnumerable<UserNotification> GetUserNotifications(string userId)
        {
            return _context.UserNotification
                .Where(un => !un.IsRead && un.UserId == userId);
        }
    }
}
using GigHub.Core;
using GigHub.Core.Repositories;
using GigHub.Persistance.Repositories;

namespace GigHub.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGigRepository Gigs { get; private set; }

        public IAttendanceRepository Attendances { get; private set; }

        public IGenreRepository Genres { get; private set; }

        public IFollowingRepository Followings { get; private set; }

        public INotificationRepository Notifications { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Gigs = new GigRepository(_context);
            Attendances = new AttendanceRepository(_context);
            Genres = new GenreRepository(_context);
            Followings = new FollowingRepository(_context);
            Notifications = new NotificationRepository(_context);
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
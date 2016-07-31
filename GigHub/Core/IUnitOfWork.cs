using GigHub.Core.Repositories;

namespace GigHub.Core
{
    public interface IUnitOfWork
    {
        IAttendanceRepository Attendances { get; }

        IFollowingRepository Followings { get; }

        IGenreRepository Genres { get; }

        IGigRepository Gigs { get; }

        INotificationRepository Notifications { get; }

        IUserNotificationRepository UserNotifications { get; }

        void Complete();
    }
}
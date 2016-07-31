using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistance;
using GigHub.Persistance.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Tests.Persistance.Repositories
{
    [TestClass]
    public class NotificationRepositoryTests
    {
        private NotificationRepository _repository;

        private Mock<DbSet<UserNotification>> _mockUserNotifications;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockUserNotifications = new Mock<DbSet<UserNotification>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.UserNotification).Returns(_mockUserNotifications.Object);

            _repository = new NotificationRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetNotificationsWithArtist_NotificationIsRead_ShouldNotBeReturned()
        {
            var notification = Notification.GigCancelled(new Gig());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);
            userNotification.Read();

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNotificationsWithArtist(user.Id);

            notifications.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNotificationsWithArtist_NotificationForOtherUser_ShouldNotBeReturned()
        {
            var notification = Notification.GigCancelled(new Gig());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNotificationsWithArtist(user.Id + "-");

            notifications.Should().BeEmpty();
        }

        [TestMethod]
        public void GetNotificationsWithArtist_NotificationIForCurrentUser_ShouldBeReturned()
        {
            var notification = Notification.GigCancelled(new Gig());
            var user = new ApplicationUser { Id = "1" };
            var userNotification = new UserNotification(user, notification);

            _mockUserNotifications.SetSource(new[] { userNotification });

            var notifications = _repository.GetNotificationsWithArtist(user.Id);

            notifications.Should().HaveCount(1);
            notifications.First().Should().Be(notification);
        }
    }
}

﻿using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _controller;

        private Mock<IGigRepository> _mockRepository;

        private string _userId;

        private int _gigId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IGigRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Gigs).Returns(_mockRepository.Object);

            _controller = new GigsController(mockUoW.Object);

            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@user.com");

            _gigId = 1;
        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExists_ShouldReturnNotFound()
        {
            var result = _controller.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCancelled_ShouldReturnNotFound()
        {
            var gig = new Gig();
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(_gigId)).Returns(gig);

            var result = _controller.Cancel(_gigId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancellingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig()
            {
                ArtistId = _userId + "-"
            };

            _mockRepository.Setup(r => r.GetGigWithAttendees(_gigId)).Returns(gig);

            var result = _controller.Cancel(_gigId);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig()
            {
                ArtistId = _userId
            };

            _mockRepository.Setup(r => r.GetGigWithAttendees(_gigId)).Returns(gig);

            var result = _controller.Cancel(_gigId);

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldSetIsCanceledToTrue()
        {
            var gig = new Gig()
            {
                ArtistId = _userId
            };

            _mockRepository.Setup(r => r.GetGigWithAttendees(_gigId)).Returns(gig);

            _controller.Cancel(_gigId);

            gig.IsCanceled.Should().BeTrue();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldNotifyAllAttendees()
        {
            var gig = new Gig()
            {
                ArtistId = _userId,
            };

            gig.Attendances.Add(new Attendance()
            {
                Attendee = new ApplicationUser()
            });

            _mockRepository.Setup(r => r.GetGigWithAttendees(_gigId)).Returns(gig);

            _controller.Cancel(_gigId);

            gig.Attendances
                .All(a => a.Attendee.UserNotifications.Any())
                .Should().BeTrue();
        }
    }
}

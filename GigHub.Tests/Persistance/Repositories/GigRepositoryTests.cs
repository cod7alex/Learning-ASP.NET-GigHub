using FluentAssertions;
using GigHub.Core.Models;
using GigHub.Persistance;
using GigHub.Persistance.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Tests.Persistance.Repositories
{
    [TestClass]
    public class GigRepositoryTests
    {
        private GigRepository _repository;

        private Mock<DbSet<Gig>> _mockGigs;

        private Mock<DbSet<Attendance>> _mockAttendances;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockGigs = new Mock<DbSet<Gig>>();
            _mockAttendances = new Mock<DbSet<Attendance>>();

            var mockContext = new Mock<IApplicationDbContext>();
            mockContext.SetupGet(c => c.Gigs).Returns(_mockGigs.Object);
            mockContext.SetupGet(c => c.Attendances).Returns(_mockAttendances.Object);

            _repository = new GigRepository(mockContext.Object);
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsInThePast_ShouldNotBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(-1),
                ArtistId = "1"
            };

            _mockGigs.SetSource(new[] { gig });
            var result = _repository.GetArtistUpcomingGigs("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsCancelled_ShouldNotBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1",
            };
            gig.Cancel();

            _mockGigs.SetSource(new[] { gig });
            var result = _repository.GetArtistUpcomingGigs("1");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_GigIsForADifferentArtist_ShouldNotBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1",
            };

            _mockGigs.SetSource(new[] { gig });
            var result = _repository.GetArtistUpcomingGigs(gig.ArtistId + "-");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetUpcomingGigsByArtist_ValidGig_ShouldBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1),
                ArtistId = "1",
            };

            _mockGigs.SetSource(new[] { gig });
            var result = _repository.GetArtistUpcomingGigs(gig.ArtistId);

            result.Should().Contain(gig);
        }

        [TestMethod]
        public void GetGigsUserIsAttending_GigIsInPast_ShouldNotBeReturned()
        {
            var attendance = new Attendance
            {
                Gig = new Gig
                {
                    DateTime = DateTime.Now.AddDays(-1)
                },
                AttendeeId = "1"
            };

            _mockAttendances.SetSource(new[] { attendance });

            var result = _repository.GetGigsUserIsAttending(attendance.AttendeeId);

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserIsAttending_NotAttendingGig_ShouldNotBeReturned()
        {
            var attendance = new Attendance
            {
                Gig = new Gig
                {
                    DateTime = DateTime.Now.AddDays(-1)
                },
                AttendeeId = "1"
            };

            _mockAttendances.SetSource(new[] { attendance });

            var result = _repository.GetGigsUserIsAttending(attendance.AttendeeId + "-");

            result.Should().BeEmpty();
        }

        [TestMethod]
        public void GetGigsUserIsAttending_AttendingGig_ShouldBeReturned()
        {
            var gig = new Gig
            {
                DateTime = DateTime.Now.AddDays(1)
            };

            var attendance = new Attendance
            {
                Gig = gig,
                AttendeeId = "1"
            };

            _mockAttendances.SetSource(new[] { attendance });

            var result = _repository.GetGigsUserIsAttending(attendance.AttendeeId);

            result.Should().HaveCount(1);
            result.First().Should().Be(gig);
        }
    }
}

using FluentAssertions;
using GigHub.Controllers;
using GigHub.Core.Models;
using GigHub.Core.ViewModels;
using GigHub.IntegrationTests.Extensions;
using GigHub.Persistance;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GigHub.IntegrationTests.Controllers
{
    [TestFixture]
    public class GigsControllerTests
    {
        private GigsController _controller;

        private ApplicationDbContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext();
            _controller = new GigsController(new UnitOfWork(_context));
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _context.Dispose();
        }

        [Test, Isolated]
        public void Mine_WhenCalled_ShouldReturnUpcomingGigs()
        {
            // Arrange
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var gig = new Gig
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = _context.Genres.First(),
                Venue = "-"
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            //Act
            var result = _controller.Mine();

            //Assert
            (result.ViewData.Model as IEnumerable<Gig>).Should().HaveCount(1);
        }

        [Test, Isolated]
        public void Update_WhenCalled_ShouldUpdateTheGivenGig()
        {
            // Arrange
            var user = _context.Users.First();
            _controller.MockCurrentUser(user.Id, user.UserName);

            var genre = _context.Genres.Single(g => g.Id == 1);
            var gig = new Gig
            {
                Artist = user,
                DateTime = DateTime.Now.AddDays(1),
                Genre = genre,
                Venue = "-"
            };

            _context.Gigs.Add(gig);
            _context.SaveChanges();

            //Act
            var result = _controller.Update(new GigFormViewModel
            {
                Id = gig.Id,
                Date = DateTime.Today.AddMonths(1).ToString("d MMM yyyy", CultureInfo.InvariantCulture),
                Time = "20:00",
                Venue = "Venue",
                Genre = 2
            });

            //Assert
            _context.Entry(gig).Reload();
            gig.DateTime.Should().Be(DateTime.Today.AddMonths(1).AddHours(20));
            gig.Venue.Should().Be("Venue");
            gig.GenreId.Should().Be(2);
        }
    }
}

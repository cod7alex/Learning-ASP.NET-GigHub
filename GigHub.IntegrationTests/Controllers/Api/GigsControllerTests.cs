using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core.Models;
using GigHub.Persistance;
using GigHub.Tests.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace GigHub.IntegrationTests.Controllers.Api
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
        public void Cancel_WhenCalled_ShouldSetIsCancelled()
        {
            //Arrange
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
            _controller.Cancel(gig.Id);

            //Assert
            _context.Entry(gig).Reload();
            gig.IsCanceled.Should().BeTrue();
        }
    }
}

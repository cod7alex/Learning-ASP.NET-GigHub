using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Tests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Http.Results;

namespace GigHub.Tests.Controllers.Api
{
    [TestClass]
    public class AttendancesControllerTests
    {
        private AttendancesController _controller;

        private Mock<IAttendanceRepository> _mockRepository;

        private string _userId;

        private int _gigId;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRepository = new Mock<IAttendanceRepository>();

            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.SetupGet(u => u.Attendances).Returns(_mockRepository.Object);

            _controller = new AttendancesController(mockUoW.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user@user.com");
        }

        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk()
        {
            var attendanceDto = new AttendanceDto()
            {
                GigId = _gigId
            };

            var result = _controller.Attend(attendanceDto);
            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void Attend_AttendanceAlreadyExists_ShouldReturnBadRequest()
        {
            var attendanceDto = new AttendanceDto()
            {
                GigId = _gigId
            };

            _mockRepository
                .Setup(r => r.GetAttendance(_gigId, _userId))
                .Returns(new Attendance());

            var result = _controller.Attend(attendanceDto);
            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            _mockRepository
                .Setup(r => r.GetAttendance(_gigId, _userId))
                .Returns(new Attendance());

            var result = _controller.DeleteAttendance(_gigId);
            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [TestMethod]
        public void DeleteAttendance_AttendanceDoesNotExist_ShouldReturnNotFound()
        {
            var result = _controller.DeleteAttendance(_gigId);
            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnTheIdOfDeletedAttendance()
        {
            var attendanceDto = new AttendanceDto()
            {
                GigId = _gigId
            };

            _mockRepository
                .Setup(r => r.GetAttendance(_gigId, _userId))
                .Returns(new Attendance());

            var result = (OkNegotiatedContentResult<int>)_controller.DeleteAttendance(_gigId);

            result.Content.Should().Be(_gigId);
        }
    }
}

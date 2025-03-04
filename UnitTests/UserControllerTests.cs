using Application.DTOs;
using Application.Interfaces.Services;
using DevChallenge.Controllers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly Mock<IValidator<UserDTO>> _validatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _validatorMock = new Mock<IValidator<UserDTO>>();
            _controller = new UserController(_loggerMock.Object, _userServiceMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            var users = new List<UserDTO> { new UserDTO { Id = 1, FirstName = "Pedro" } };
            _userServiceMock.Setup(s => s.GetUsers()).ReturnsAsync(users);

            var result = await _controller.GetUsers();
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            var user = new UserDTO { Id = 1, FirstName = "Pedro" };
            _userServiceMock.Setup(s => s.GetUserById(1)).ReturnsAsync(user);

            var result = await _controller.GetUserById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsBadRequest_WhenValidationFails()
        {
            var user = new UserDTO { Id = 1, FirstName = "Pedro" };
            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("FirstName", "FirstName is required") });
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(validationResult);

            var result = await _controller.CreateUser(user);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("FirstName is required", (IEnumerable<string>)badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUser_ReturnsCreated_WhenUserIsCreated()
        {
            var user = new UserDTO { Id = 1, FirstName = "Pedro" };
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(s => s.CreateUser(user)).ReturnsAsync(true);

            var result = await _controller.CreateUser(user);
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task UpdateUser_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            var user = new UserDTO { Id = 1, FirstName = "Pedro" };
            _validatorMock.Setup(v => v.ValidateAsync(user, default)).ReturnsAsync(new ValidationResult());
            _userServiceMock.Setup(s => s.UpdateUser(1, user)).ReturnsAsync(true);

            var result = await _controller.UpdateUser(1, user);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContent()
        {
            _userServiceMock.Setup(s => s.DeleteUser(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteUser(1);
            Assert.IsType<NoContentResult>(result);
        }

    }
}
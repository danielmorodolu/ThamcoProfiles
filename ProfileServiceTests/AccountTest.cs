using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileService.Controllers;
using ProfileService.Models;
using ProfileService.Services.Profiling;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ProfileService.Tests.Controllers
{
    public class AccountTest
    {
        private readonly Mock<IProfileService> _mockProfileService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<ILogger<AccountController>> _mockLogger;
        private readonly AccountController _controller;

        public AccountTest()
        {
            _mockProfileService = new Mock<IProfileService>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockLogger = new Mock<ILogger<AccountController>>();

            _controller = new AccountController(
                _mockConfiguration.Object,
                _mockProfileService.Object,
                _mockLogger.Object
            );
        }

        [Fact]
        public async Task Details_UserExists_ReturnsViewResult()
        {
            // Arrange
            var auth0UserId = "test-user-id";
            var user = new Profile
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password99$",
                Auth0UserId = auth0UserId
            };

            _mockProfileService.Setup(service => service.GetUserByAuth0IdAsync(auth0UserId))
                .ReturnsAsync(user);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, auth0UserId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            // Act
            var result = await _controller.Details();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Profile>(viewResult.Model);
            Assert.Equal(user.FirstName, model.FirstName);
        }

        [Fact]
        public async Task EditField_ValidField_ReturnsViewResult()
        {
            // Arrange
            var field = "FirstName";
            var user = new Profile
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "Password99$"
            };

            _mockProfileService.Setup(service => service.GetUserByAuth0IdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            // Act
            var result = await _controller.EditField(field);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Profile>(viewResult.Model);
            Assert.Equal(user.FirstName, model.FirstName);
        }

        [Fact]
        public async Task EditField_UserNotFound_ReturnsNotFoundResult()
        {
            // Arrange
            var field = "FirstName";
            var auth0UserId = "test-user-id";

            _mockProfileService.Setup(service => service.GetUserByAuth0IdAsync(auth0UserId))
                .ReturnsAsync((Profile?)null); // Nullable fix

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, auth0UserId)
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            // Act
            var result = await _controller.EditField(field);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditField_Post_ValidData_ReturnsRedirectToActionResult()
        {
            // Arrange
            var field = "FirstName";
            var newValue = "Jane";
            var user = new Profile
            {
                Id = 1,
                FirstName = "John",
                Email = "john.doe@example.com",
                Password = "Password99$"
            };

            _mockProfileService.Setup(service => service.GetUserByAuth0IdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userPrincipal }
            };

            // Act
            var result = await _controller.EditField(field, newValue);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", redirectResult.ActionName);
        }
    }
}

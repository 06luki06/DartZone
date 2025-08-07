using System;
using System.Security.Claims;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Controllers;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.Tests.API.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private DartZoneDbContext _dbContext = null!;
        private AuthController _controller;
        private const string SecretKey = "SuperSecretJwtKey12345!6789@!012";
        private const string Issuer = "TestIssuer";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Environment.SetEnvironmentVariable("JWT_KEY", SecretKey);
            Environment.SetEnvironmentVariable("JWT_ISSUER", Issuer);
        }

        [SetUp]
        public void SetUp()
        {
            _dbContext = HelperMethods.CreateDbContext();
            IServiceProvider serviceProvider = HelperMethods.CreateServiceProvider();

            ValidatorFactory validatorFactory = new(serviceProvider);
            DtoMapperFactory mapperFactory = new(serviceProvider);

            _controller = new(_dbContext, validatorFactory, mapperFactory);
        }

        [TearDown]
        public void TearDown()
        {
            if (_dbContext is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public async Task Register_ShouldCreateUser_AndReturnToken()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            IActionResult result = await _controller.Register(dto);

            CreatedResult created = result as CreatedResult;
            TokenResponseDto tokenResponse = created.Value as TokenResponseDto;
            tokenResponse.Token.Should().StartWith("eyJ");
        }

        [Test]
        public async Task Register_ShouldReturnBadRequest_WhenValidationFails()
        {
            UserRequestDto dto = new() { Username = "", Password = "Secure123!" };
            IActionResult result = await _controller.Register(dto);

            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            badRequest.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = badRequest.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("Username is required.");
        }

        [Test]
        public async Task Register_ShouldReturnBadRequest_WhenUsernameExists()
        {
            UserRequestDto dto = new() { Username = "existinguser", Password = "Secure123!" };
            _dbContext.Users.Add(new User(dto.Username, [], []));
            await _dbContext.SaveChangesAsync();
            IActionResult result = await _controller.Register(dto);

            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            badRequest.Value.Should().BeOfType<MessageResponseDto>();
        }

        [Test]
        public async Task Login_ShouldReturnTokenResponseDto_WhenCredentialsAreValid()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };

            await _controller.Register(dto);
            IActionResult result = await _controller.Login(dto);
            OkObjectResult okResult = result as OkObjectResult;

            TokenResponseDto tokenResponse = okResult.Value as TokenResponseDto;
            tokenResponse.Token.Should().StartWith("eyJ");
        }

        [Test]
        public async Task Login_ShouldReturnBadRequest_WhenValidationFails()
        {
            UserRequestDto dto = new() { Username = "", Password = "Secure123!" };

            IActionResult result = await _controller.Login(dto);
            BadRequestObjectResult badRequest = result as BadRequestObjectResult;
            badRequest.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = badRequest.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("Username is required.");
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenUserNameDoesNotExist()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _controller.Register(dto);

            UserRequestDto dto2 = new() { Username = "testuser2", Password = "Secure123!" };
            IActionResult result = await _controller.Login(dto2);

            UnauthorizedObjectResult unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = unauthorized.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("Invalid username or password.");
        }

        [Test]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsIncorrect()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _controller.Register(dto);

            UserRequestDto dto2 = new() { Username = "testuser", Password = "Secure123!!" };
            IActionResult result = await _controller.Login(dto2);

            UnauthorizedObjectResult unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = unauthorized.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("Invalid username or password.");
        }

        [Test]
        public async Task GetCurrentUser_ShouldReturnUser_WhenTokenValid()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _controller.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _controller.ControllerContext = HelperMethods.CreateControllerContext(user);

            IActionResult result = await _controller.GetCurrentUser();
            OkObjectResult ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            ok.Value.Should().BeOfType<UserResponseDto>();
            UserResponseDto userResponse = ok.Value as UserResponseDto;
            userResponse.Username.Should().Be(dto.Username);
            userResponse.Id.Should().Be(user.Id);
        }

        [Test]
        public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenTokenInvalid()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            IActionResult result = await _controller.GetCurrentUser();
            UnauthorizedObjectResult unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = unauthorized.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("User not authenticated.");
        }

        [Test]
        public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            User user = new("nonexistentuser", [], []);
            _controller.ControllerContext = HelperMethods.CreateControllerContext(user);
            IActionResult result = await _controller.GetCurrentUser();
            UnauthorizedObjectResult unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = unauthorized.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("User not authenticated.");
        }

        [Test]
        public async Task DeleteUser_ShouldDeleteUser_WhenUserExists()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _controller.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _controller.ControllerContext = HelperMethods.CreateControllerContext(user);
            IActionResult result = await _controller.DeleteUser();
            result.Should().BeOfType<NoContentResult>();
            User deletedUser = await _dbContext.Users.FindAsync(user.Id);
            deletedUser.Should().BeNull();
        }

        [Test]
        public async Task DeleteUser_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            IActionResult result = await _controller.DeleteUser();
            UnauthorizedObjectResult unauthorized = result as UnauthorizedObjectResult;
            unauthorized.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = unauthorized.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("User not authenticated.");
        }

        [Test]
        public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            User user = new("nonexistentuser", [], []);
            _controller.ControllerContext = HelperMethods.CreateControllerContext(user);
            IActionResult result = await _controller.DeleteUser();
            UnauthorizedObjectResult notFound = result as UnauthorizedObjectResult;
            notFound.Value.Should().BeOfType<MessageResponseDto>();
            MessageResponseDto messageResponse = notFound.Value as MessageResponseDto;
            messageResponse.Message.Should().Be("User not authenticated.");
        }
    }
}
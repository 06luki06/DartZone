﻿using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.Configuration;

namespace At.luki0606.DartZone.Tests.API.Controllers
{
    [TestFixture]
    public class GamesControllerTest
    {
        private GamesController _gameController;
        private AuthController _authController;
        private DartZoneDbContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = HelperMethods.CreateDbContext();
            IServiceProvider serviceProvider = HelperMethods.CreateServiceProvider();
            IConfiguration config = HelperMethods.CreateConfiguration();

            ValidatorFactory validatorFactory = new(serviceProvider);
            DtoMapperFactory mapperFactory = new(serviceProvider);

            _gameController = new(_dbContext, mapperFactory);
            _authController = new(_dbContext, config, validatorFactory, mapperFactory);
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
        public async Task AddGame_ShouldReturnCreated_WhenUserIsAuthenticated()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _authController.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _gameController.ControllerContext = HelperMethods.CreateControllerContext(user);

            IActionResult result = await _gameController.AddGame();
            result.Should().BeOfType<CreatedAtActionResult>();
            CreatedAtActionResult createdResult = result as CreatedAtActionResult;
            createdResult.Value.Should().BeOfType<GameResponseDto>();
        }

        [Test]
        public async Task AddGame_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            _gameController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            IActionResult result = await _gameController.AddGame();
            result.Should().BeOfType<UnauthorizedObjectResult>();
            UnauthorizedObjectResult unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Value.Should().BeOfType<MessageResponseDto>();
        }

        [Test]
        public async Task AddGame_ShouldReturnBadRequest_WhenInvalidUserId()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _authController.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _gameController.ControllerContext = HelperMethods.CreateControllerContext(user);

            _gameController.ControllerContext.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(
                [
                    new(ClaimTypes.NameIdentifier, "invalid-id"),
                    new(ClaimTypes.Name, user.Username)
                ], "TestAuth"));

            IActionResult result = await _gameController.AddGame();
            result.Should().BeOfType<UnauthorizedObjectResult>();
            UnauthorizedObjectResult badRequestResult = result as UnauthorizedObjectResult;
            badRequestResult.Value.Should().BeOfType<MessageResponseDto>();
        }

        [Test]
        public async Task GetGames_ShouldReturnOk_WhenUserIsAuthenticated()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _authController.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _gameController.ControllerContext = HelperMethods.CreateControllerContext(user);

            await _gameController.AddGame();
            IActionResult result = await _gameController.GetGames();
            result.Should().BeOfType<OkObjectResult>();
            OkObjectResult okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<List<GameResponseDto>>();
            List<GameResponseDto> games = okResult.Value as List<GameResponseDto>;
            games.Should().HaveCount(1);
        }

        [Test]
        public async Task GetGames_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            _gameController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            IActionResult result = await _gameController.GetGames();
            result.Should().BeOfType<UnauthorizedObjectResult>();
            UnauthorizedObjectResult unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Value.Should().BeOfType<MessageResponseDto>();
        }

        [Test]
        public async Task GetGame_ShouldReturnGame_WhenGameExists()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _authController.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _gameController.ControllerContext = HelperMethods.CreateControllerContext(user);

            IActionResult addedGameResult = await _gameController.AddGame();
            GameResponseDto addedGame = (addedGameResult as CreatedAtActionResult).Value as GameResponseDto;
            IActionResult result = await _gameController.GetGame(addedGame.Id);
            result.Should().BeOfType<OkObjectResult>();
            OkObjectResult okResult = result as OkObjectResult;
            okResult.Value.Should().BeOfType<GameResponseDto>();
            GameResponseDto game = okResult.Value as GameResponseDto;
            game.Id.Should().Be(addedGame.Id);
        }

        [Test]
        public async Task GetGame_ShouldReturnNotFound_WhenGameDoesNotExist()
        {
            UserRequestDto dto = new() { Username = "testuser", Password = "Secure123!" };
            await _authController.Register(dto);

            User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            _gameController.ControllerContext = HelperMethods.CreateControllerContext(user);

            IActionResult result = await _gameController.GetGame(Guid.NewGuid());
            result.Should().BeOfType<NotFoundObjectResult>();
            NotFoundObjectResult notFoundResult = result as NotFoundObjectResult;
            notFoundResult.Value.Should().BeOfType<MessageResponseDto>();
        }

        [Test]
        public async Task GetGame_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
        {
            _gameController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };
            IActionResult result = await _gameController.GetGame(Guid.NewGuid());
            result.Should().BeOfType<UnauthorizedObjectResult>();
            UnauthorizedObjectResult unauthorizedResult = result as UnauthorizedObjectResult;
            unauthorizedResult.Value.Should().BeOfType<MessageResponseDto>();
        }
    }
}

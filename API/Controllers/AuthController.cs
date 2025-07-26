using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Helpers;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace At.luki0606.DartZone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DartZoneDbContext _db;
        private readonly IConfiguration _config;
        private readonly IDtoMapperFactory _mapperFactory;
        private readonly Validators.IValidatorFactory _validationFactory;

        public AuthController(DartZoneDbContext db, IConfiguration config, Validators.IValidatorFactory validatorFactory, IDtoMapperFactory mapperFactory)
        {
            _db = db;
            _config = config;
            _validationFactory = validatorFactory;
            _mapperFactory = mapperFactory;
        }

        #region POST
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestDto dto)
        {
            ValidationResult validationResult = await _validationFactory.GetValidator<UserRequestDto>().ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationResultHelper.GetFirstErrorMessage(validationResult));
            }

            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username already exists.");
            }

            (byte[] hash, byte[] salt) = PasswordHasher.HashPassword(dto.Password);

            User user = new(dto.Username, hash, salt);

            _db.Users.Add(user);
            try
            {
                await _db.SaveChangesAsync();
                TokenResponseDto token = new()
                {
                    Token = JwtService.GenerateToken(user, _config)
                };
                return Created(nameof(GetCurrentUser), token);
            }
            catch (Exception)
            {
                return StatusCode(500, new MessageResponseDto() { Message = "An error occurred while registering the user." });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserRequestDto dto)
        {
            ValidationResult validationResult = await _validationFactory.GetValidator<UserRequestDto>().ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationResultHelper.GetFirstErrorMessage(validationResult));
            }

            User user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized(new MessageResponseDto() { Message = "Invalid username or password." });
            }

            using HMACSHA512 hmac = new(user.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password));
            if (!computedHash.AsSpan().SequenceEqual(user.PasswordHash))
            {
                return Unauthorized(new MessageResponseDto() { Message = "Invalid username or password." });
            }

            TokenResponseDto token = new()
            {
                Token = JwtService.GenerateToken(user, _config)
            };
            return Ok(token);
        }

        #region GET
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = await _db.Users.FindAsync(Guid.Parse(userId));

            if (user == null)
            {
                return NotFound(new MessageResponseDto() { Message = "User not found." });
            }

            UserResponseDto userResponse = _mapperFactory.GetMapper<User, UserResponseDto>().Map(user);
            return Ok(userResponse);
        }
        #endregion

        #region DELETE
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = await _db.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound(new MessageResponseDto() { Message = "User not found." });
            }
            _db.Users.Remove(user);
            try
            {
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new MessageResponseDto() { Message = "An error occurred while deleting the user." });
            }
        }
        #endregion
    }
}

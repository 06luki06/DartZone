using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Helpers;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Results;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        public AuthController(DartZoneDbContext db, IValidatorFactory validatorFactory, IDtoMapperFactory mapperFactory)
            : base(db, mapperFactory, validatorFactory)
        {
        }

        #region POST
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRequestDto dto)
        {
            ValidationResult validationResult = await _validationFactory.GetValidator<UserRequestDto>().Value.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(ValidationResultHelper.GetFirstErrorMessage(validationResult));
            }

            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest(new MessageResponseDto() { Message = "Username already exists." });
            }

            (byte[] hash, byte[] salt) = PasswordHasherService.HashPassword(dto.Password);

            User user = new(dto.Username, hash, salt);

            _db.Users.Add(user);
            try
            {
                await _db.SaveChangesAsync();
                TokenResponseDto token = new()
                {
                    Token = JwtService.GenerateToken(user)
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
            ValidationResult validationResult = await _validationFactory.GetValidator<UserRequestDto>().Value.ValidateAsync(dto);
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
            if (PasswordHasherService.VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt).IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "Invalid username or password." });
            }

            TokenResponseDto token = new()
            {
                Token = JwtService.GenerateToken(user)
            };
            return Ok(token);
        }
        #endregion

        #region GET
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = userResult.Value;

            UserResponseDto userResponse = _dtoMapperFactory.GetMapper<User, UserResponseDto>().Value.Map(user);
            return Ok(userResponse);
        }
        #endregion

        #region DELETE
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = userResult.Value;

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

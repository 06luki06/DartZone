using System;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Dtos;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
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

        public AuthController(DartZoneDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        #region POST
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthenticationDto dto)
        {
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
                string token = JwtService.GenerateToken(user, _config);
                return Ok(new { token });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthenticationDto dto)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            using HMACSHA512 hmac = new(user.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dto.Password));
            if (!computedHash.AsSpan().SequenceEqual(user.PasswordHash))
            {
                return Unauthorized("Invalid username or password.");
            }

            string token = JwtService.GenerateToken(user, _config);
            return Ok(new { token });
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult Logout()
        {
            // In a stateless JWT authentication system, logout is typically handled on the client side.
            return Ok("User logged out successfully.");
        }
        #endregion

        #region GET
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }
            User user = await _db.Users.FindAsync(Guid.Parse(userId));
            return user == null
                ? NotFound("User not found.")
                : Ok(new
                {
                    user.Id,
                    user.Username
                });
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
                return Unauthorized("User not authenticated.");
            }
            User user = await _db.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound("User not found.");
            }
            _db.Users.Remove(user);
            try
            {
                await _db.SaveChangesAsync();
                return Ok("User deleted successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
        #endregion
    }
}

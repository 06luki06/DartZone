using System;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Models.Dtos;
using At.luki0606.DartZone.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DartZoneDbContext _db;

        public AuthController(DartZoneDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            {
                return BadRequest("Username already exists.");
            }

            (byte[] hash, byte[] salt) = PasswordHasher.HashPassword(dto.Password);

            User user = new()
            {
                Username = dto.Username,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            _db.Users.Add(user);
            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { user.Id, user.Username });
            }
            catch (Exception)
            {
                // Log the exception (ex) here if needed
                return StatusCode(500, "An error occurred while registering the user.");
            }
        }
    }
}

using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Dtos;
using At.luki0606.DartZone.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DartZoneDbContext _db;

        public GamesController(DartZoneDbContext db)
        {
            _db = db;
        }

        #region POST
        [HttpPost("")]
        public async Task<IActionResult> AddGame()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            Game game = new(parsedUserId, 301);

            _db.Games.Add(game);
            try
            {
                await _db.SaveChangesAsync();
                return Ok(new { game.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding the game: {ex.Message}");
            }
        }
        #endregion

        #region GET
        [HttpGet("")]
        public async Task<IActionResult> GetGames()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }
            var games = await _db.Games
                .Where(g => g.UserId == parsedUserId)
                    .Include(g => g.Throws)
                        .ThenInclude(t => t.Darts)
                .Select(g => new GameDto(g))
                .ToListAsync();
            return Ok(games);
        }
        #endregion
    }
}

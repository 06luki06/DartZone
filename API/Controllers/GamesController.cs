using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly DartZoneDbContext _db;
        private readonly IDtoMapperFactory _mapperFactory;

        public GamesController(DartZoneDbContext db, IDtoMapperFactory mapperFactory)
        {
            _db = db;
            _mapperFactory = mapperFactory;
        }

        #region POST
        [HttpPost("")]
        public async Task<IActionResult> AddGame()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new MessageResponseDto() { Message = "Invalid user ID." });
            }

            Game game = new(parsedUserId, 301);

            _db.Games.Add(game);
            try
            {
                await _db.SaveChangesAsync();
                GameResponseDto gameResponse = _mapperFactory.GetMapper<Game, GameResponseDto>().Map(game);
                return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameResponse);
            }
            catch (Exception)
            {
                return StatusCode(500, new MessageResponseDto() { Message = "An error occurred while adding the game" });
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
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new MessageResponseDto() { Message = "Invalid user ID." });
            }
            List<GameResponseDto> games = await _db.Games
                .Where(g => g.UserId == parsedUserId)
                    .Include(g => g.Throws)
                        .ThenInclude(t => t.Darts)
                .Select(g => _mapperFactory.GetMapper<Game, GameResponseDto>().Map(g))
                .ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return BadRequest(new MessageResponseDto() { Message = "Invalid user ID." });
            }
            Game game = await _db.Games
                .Where(g => g.Id == id && g.UserId == parsedUserId)
                    .Include(g => g.Throws)
                        .ThenInclude(t => t.Darts)
                .FirstOrDefaultAsync();
            if (game == null)
            {
                return NotFound(new MessageResponseDto() { Message = "Game not found." });
            }
            GameResponseDto gameResponse = _mapperFactory.GetMapper<Game, GameResponseDto>().Map(game);
            return Ok(gameResponse);
        }
        #endregion
    }
}

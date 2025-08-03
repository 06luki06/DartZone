using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : BaseController
    {
        public GamesController(DartZoneDbContext db, IDtoMapperFactory mapperFactory)
            : base(db, mapperFactory)
        {
        }

        #region POST
        [HttpPost("")]
        public async Task<IActionResult> AddGame()
        {
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = userResult.Value;

            Game game = new(user.Id, 301);

            _db.Games.Add(game);
            try
            {
                await _db.SaveChangesAsync();
                GameResponseDto gameResponse = _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(game);
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
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = userResult.Value;

            List<GameResponseDto> games = await _db.Games
                .Where(g => g.UserId == user.Id)
                    .Include(g => g.Throws)
                        .ThenInclude(t => t.Darts)
                .Select(g => _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(g))
                .ToListAsync();
            return Ok(games);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame(Guid id)
        {
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }
            User user = userResult.Value;

            Game game = await _db.Games
                .Where(g => g.Id == id && g.UserId == user.Id)
                    .Include(g => g.Throws)
                        .ThenInclude(t => t.Darts)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound(new MessageResponseDto() { Message = "Game not found." });
            }

            GameResponseDto gameResponse = _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(game);
            return Ok(gameResponse);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            Result<User> userResult = await GetAuthenticatedUser();
            if (userResult.IsFailure)
            {
                return Unauthorized(new MessageResponseDto() { Message = "User not authenticated." });
            }

            User user = userResult.Value;
            Game game = await _db.Games
                .Where(g => g.Id == id && g.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound(new MessageResponseDto() { Message = "Game not found." });
            }

            _db.Games.Remove(game);
            try
            {
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new MessageResponseDto() { Message = "An error occurred while deleting the game" });
            }
        }
        #endregion
    }
}

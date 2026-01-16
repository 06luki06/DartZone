using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Constants;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Helpers;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using At.luki0606.DartZone.Shared.Results;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace At.luki0606.DartZone.API.Controllers;

[Route("api/[controller]")]
[ApiController]
internal sealed class GamesController : BaseController
{
    public GamesController(DartZoneDbContext db, IDtoMapperFactory mapperFactory, IValidatorFactory validatorFactory)
        : base(db, mapperFactory, validatorFactory)
    {
    }

    #region POST
    [HttpPost("")]
    public async Task<IActionResult> AddGame()
    {
        Result<User> userResult = await GetAuthenticatedUser().ConfigureAwait(false);
        if (userResult.IsFailure)
        {
            return Unauthorized(new MessageResponseDto() { Message = Phrases.UserNotAuthenticated });
        }
        User user = userResult.Value;

        Game game = new(user.Id, 301);

        _db.Games.Add(game);
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await _db.SaveChangesAsync().ConfigureAwait(false);
            GameResponseDto gameResponse = _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameResponse);
        }
        catch (Exception)
        {
            return StatusCode(500, new MessageResponseDto() { Message = Phrases.AnErrocOccured });
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    [HttpPost("{id}/throws")]
    public async Task<IActionResult> AddThrow(Guid id, [FromBody] ThrowRequestDto throwRequest)
    {
        Result<User> userResult = await GetAuthenticatedUser().ConfigureAwait(false);
        if (userResult.IsFailure)
        {
            return Unauthorized(new MessageResponseDto() { Message = Phrases.UserNotAuthenticated });
        }
        User user = userResult.Value;

        ValidationResult validationResult = await _validationFactory
            .GetValidator<ThrowRequestDto>().Value.ValidateAsync(throwRequest)
            .ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            return BadRequest(ValidationResultHelper.GetFirstErrorMessage(validationResult));
        }

        Game game = await _db.Games
            .Where(g => g.Id == id && g.UserId == user.Id)
                .Include(g => g.Throws)
                    .ThenInclude(t => t.Darts)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        if (game == null)
        {
            return NotFound(new MessageResponseDto() { Message = Phrases.GameNotFound });
        }

        Dart dart1 = new(throwRequest.Dart1);
        Dart dart2 = new(throwRequest.Dart2);
        Dart dart3 = new(throwRequest.Dart3);

        Throw newThrow = new(game.Id, dart1, dart2, dart3);

        Result result = game.AddThrow(newThrow);
        if (result.IsFailure)
        {
            return BadRequest(new MessageResponseDto() { Message = result.Error });
        }

        _db.Darts.AddRange(dart1, dart2, dart3);
        _db.Throws.Add(newThrow);
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await _db.SaveChangesAsync().ConfigureAwait(false);
            ThrowResponseDto throwResponse = _dtoMapperFactory.GetMapper<Throw, ThrowResponseDto>().Value.Map(newThrow);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, throwResponse);
        }
        catch (Exception)
        {
            return StatusCode(500, new MessageResponseDto() { Message = Phrases.AnErrocOccured });
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
    #endregion

    #region GET
    [HttpGet("")]
    public async Task<IActionResult> GetGames()
    {
        Result<User> userResult = await GetAuthenticatedUser().ConfigureAwait(false);
        if (userResult.IsFailure)
        {
            return Unauthorized(new MessageResponseDto() { Message = Phrases.UserNotAuthenticated });
        }
        User user = userResult.Value;

        List<GameResponseDto> games = await _db.Games
            .Where(g => g.UserId == user.Id)
                .Include(g => g.Throws)
                    .ThenInclude(t => t.Darts)
            .Select(g => _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(g))
            .ToListAsync().ConfigureAwait(false);
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGame(Guid id)
    {
        Result<User> userResult = await GetAuthenticatedUser().ConfigureAwait(false);
        if (userResult.IsFailure)
        {
            return Unauthorized(new MessageResponseDto() { Message = Phrases.UserNotAuthenticated });
        }
        User user = userResult.Value;

        Game game = await _db.Games
            .Where(g => g.Id == id && g.UserId == user.Id)
                .Include(g => g.Throws)
                    .ThenInclude(t => t.Darts)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        if (game == null)
        {
            return NotFound(new MessageResponseDto() { Message = Phrases.GameNotFound });
        }

        GameResponseDto gameResponse = _dtoMapperFactory.GetMapper<Game, GameResponseDto>().Value.Map(game);
        return Ok(gameResponse);
    }
    #endregion

    #region DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(Guid id)
    {
        Result<User> userResult = await GetAuthenticatedUser().ConfigureAwait(false);
        if (userResult.IsFailure)
        {
            return Unauthorized(new MessageResponseDto() { Message = Phrases.UserNotAuthenticated });
        }

        User user = userResult.Value;
        Game game = await _db.Games
            .Where(g => g.Id == id && g.UserId == user.Id)
            .FirstOrDefaultAsync().ConfigureAwait(false);

        if (game == null)
        {
            return NotFound(new MessageResponseDto() { Message = Phrases.GameNotFound });
        }

        _db.Games.Remove(game);
#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            await _db.SaveChangesAsync().ConfigureAwait(false);
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500, new MessageResponseDto() { Message = Phrases.AnErrocOccured });
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }
    #endregion
}

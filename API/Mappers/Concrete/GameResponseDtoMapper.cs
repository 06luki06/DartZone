using System.Linq;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;

namespace At.luki0606.DartZone.API.Mappers.Concrete;

internal class GameResponseDtoMapper : IDtoMapper<Game, GameResponseDto>
{
    private readonly IDtoMapper<Throw, ThrowResponseDto> _throwMapper;

    public GameResponseDtoMapper(IDtoMapper<Throw, ThrowResponseDto> throwMapper)
    {
        _throwMapper = throwMapper;
    }

    public GameResponseDto Map(Game entity)
    {
        return new GameResponseDto
        {
            Id = entity.Id,
            StartScore = entity.StartScore,
            CurrentScore = entity.CurrentScore,
            CreatedAt = entity.CreatedAt,
            HasFinished = entity.HasFinished,
            HasStarted = entity.HasStarted,
            Throws = [.. entity.Throws.Select(_throwMapper.Map)]
        };
    }
}

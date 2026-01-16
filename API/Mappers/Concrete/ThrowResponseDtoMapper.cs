using System.Linq;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;

namespace At.luki0606.DartZone.API.Mappers.Concrete;

internal sealed class ThrowResponseDtoMapper : IDtoMapper<Throw, ThrowResponseDto>
{
    private readonly IDtoMapper<Dart, DartResponseDto> _dartMapper;

    public ThrowResponseDtoMapper(IDtoMapper<Dart, DartResponseDto> dartMapper)
    {
        _dartMapper = dartMapper;
    }

    public ThrowResponseDto Map(Throw entity)
    {
        return new ThrowResponseDto
        {
            Id = entity.Id,
            Darts = [.. entity.Darts.Select(_dartMapper.Map)],
        };
    }
}

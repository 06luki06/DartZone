using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;

namespace At.luki0606.DartZone.API.Mappers.Concrete;

internal sealed class DartResponseDtoMapper : IDtoMapper<Dart, DartResponseDto>
{
    public DartResponseDto Map(Dart entity)
    {
        return new DartResponseDto
        {
            Id = entity.Id,
            Multiplier = entity.Multiplier,
            Field = entity.Field
        };
    }
}

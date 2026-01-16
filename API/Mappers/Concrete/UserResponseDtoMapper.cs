using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Dtos.Responses;

namespace At.luki0606.DartZone.API.Mappers.Concrete;

internal sealed class UserResponseDtoMapper : IDtoMapper<User, UserResponseDto>
{
    public UserResponseDto Map(User entity)
    {
        return new UserResponseDto
        {
            Id = entity.Id,
            Username = entity.Username
        };
    }
}

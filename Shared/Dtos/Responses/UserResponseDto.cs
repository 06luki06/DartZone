using System;

namespace At.luki0606.DartZone.Shared.Dtos.Responses;

public sealed class UserResponseDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Username { get; set; } = string.Empty;
}

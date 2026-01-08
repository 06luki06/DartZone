using System;
using System.Collections.Generic;

namespace At.luki0606.DartZone.Shared.Dtos.Responses;

public class GameResponseDto
{
    public Guid Id { get; set; } = Guid.Empty;
    public int StartScore
    {
        get; set;
    }
    public int CurrentScore
    {
        get; set;
    }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool HasFinished
    {
        get; set;
    }
    public bool HasStarted
    {
        get; set;
    }
    public IReadOnlyList<ThrowResponseDto> Throws { get; set; } = [];
}

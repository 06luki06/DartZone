using System;
using System.Collections.Generic;

namespace At.luki0606.DartZone.Shared.Dtos.Responses
{
    public class GameResponseDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public int StartScore { get; set; } = 0;
        public int CurrentScore { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool HasFinished { get; set; } = false;
        public bool HasStarted { get; set; } = false;
        public IReadOnlyList<ThrowResponseDto> Throws { get; set; } = [];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace At.luki0606.DartZone.Shared.Dtos.Responses
{
    public class ThrowResponseDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public IReadOnlyList<DartResponseDto> Darts { get; set; } = [];
        public int TotalScore => Darts.Sum(d => d.Score);
    }
}

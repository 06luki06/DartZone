using System;
using System.Collections.Generic;
using System.Linq;
using At.luki0606.DartZone.API.Models;

namespace At.luki0606.DartZone.API.Dtos
{
    public class ThrowDto
    {
        #region Properties
        public Guid Id { get; }
        public IReadOnlyList<DartDto> Darts { get; }
        public int TotalScore { get; }
        public DateTime CreatedAt { get; }
        #endregion

        #region Ctor
        public ThrowDto(Throw t)
        {
            Id = t.Id;
            Darts = [.. t.Darts.Select(d => new DartDto(d))];
            TotalScore = t.TotalScore;
            CreatedAt = t.CreatedAt;
        }
        #endregion
    }
}

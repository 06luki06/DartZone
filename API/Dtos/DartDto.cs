using System;
using At.luki0606.DartZone.API.Enums;
using At.luki0606.DartZone.API.Models;

namespace At.luki0606.DartZone.API.Dtos
{
    public class DartDto
    {
        #region Properties
        public Guid Id { get; }
        public Multiplier Multiplier { get; }
        public int Field { get; }
        public int Score { get; }
        #endregion

        #region Ctor
        public DartDto(Dart dart)
        {
            Id = dart.Id;
            Multiplier = dart.Multiplier;
            Field = dart.Field;
            Score = dart.Score;
        }
        #endregion
    }
}

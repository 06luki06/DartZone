using System;
using At.luki0606.DartZone.Shared.Enums;

namespace At.luki0606.DartZone.Shared.Dtos.Responses
{
    public class DartResponseDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Multiplier Multiplier { get; set; } = Multiplier.Single;
        public int Field { get; set; } = 0;
        public int Score => Multiplier switch
        {
            Multiplier.Single or Multiplier.Double or Multiplier.Triple => Field * (int)Multiplier,
            _ => 0
        };
    }
}

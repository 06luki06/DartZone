﻿using At.luki0606.DartZone.Shared.Enums;

namespace At.luki0606.DartZone.Shared.Dtos.Requests
{
    public class DartRequestDto
    {
        public Multiplier Multiplier { get; set; } = Multiplier.Single;
        public int Field { get; set; } = 0;
    }
}

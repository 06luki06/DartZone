using System;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Enums;

namespace At.luki0606.DartZone.Tests.API.Models;

internal static class HelperMethods
{
    internal static Throw GetSampleThrow(Guid gameId)
    {
        Dart dart1 = new(Multiplier.Single, 10);
        Dart dart2 = new(Multiplier.Double, 20);
        Dart dart3 = new(Multiplier.Triple, 5);

        return new(gameId, dart1, dart2, dart3);
    }
}

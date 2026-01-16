using At.luki0606.DartZone.API.Models;

namespace At.luki0606.DartZone.API.Services;

internal interface IJwtService
{
    string GenerateToken(User user);
}

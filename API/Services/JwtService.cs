using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using At.luki0606.DartZone.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.API.Services;

internal static class JwtService
{
    public static string GenerateToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        string key = Environment.GetEnvironmentVariable("JWT_KEY");
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            Expires = DateTime.UtcNow.AddHours(1),
            IssuedAt = DateTime.UtcNow,
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

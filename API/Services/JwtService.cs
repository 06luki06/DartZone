using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using At.luki0606.DartZone.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.API.Services;

internal sealed class JwtService : IJwtService
{
    private readonly IConfiguration _config;

    public JwtService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        JwtSecurityTokenHandler tokenHandler = new();

        string key = _config["Jwt:Key"];
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Audience = _config["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddHours(1),
            IssuedAt = DateTime.UtcNow,
            Issuer = _config["Jwt:Issuer"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

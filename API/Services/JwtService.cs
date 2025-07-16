using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using At.luki0606.DartZone.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.API.Services
{
    public static class JwtService
    {
        public static string GenerateToken(User user, IConfiguration config)
        {
            JwtSecurityTokenHandler tokenHandler = new();

            byte[] key = System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username)
                ]),
                Expires = DateTime.UtcNow.AddMinutes(1),
                IssuedAt = DateTime.UtcNow,
                Issuer = config["Jwt:Issuer"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

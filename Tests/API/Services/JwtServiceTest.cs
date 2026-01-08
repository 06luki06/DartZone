using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
using FluentAssertions;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.Tests.API.Services;

[TestFixture]
internal sealed class JwtServiceTest
{
    private const string SecretKey = "SuperSecretJwtKey12345!6789@!012";
    private const string Issuer = "TestIssuer";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Environment.SetEnvironmentVariable("JWT_KEY", SecretKey);
        Environment.SetEnvironmentVariable("JWT_ISSUER", Issuer);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        User user = new("TestUser", [], []);
        string token = JwtService.GenerateToken(user);

        token.Should().NotBeNullOrEmpty();

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwt = handler.ReadJwtToken(token);

        jwt.Issuer.Should().Be(Issuer);
    }

    [Test]
    public void GenerateToken_ShouldBeValidAccordingToValidationParameters()
    {
        User user = new("ValidUser", [], []);
        string token = JwtService.GenerateToken(user);

        byte[] key = Encoding.UTF8.GetBytes(SecretKey);

#pragma warning disable CA5404 // Do not disable token validation checks
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
#pragma warning restore CA5404 // Do not disable token validation checks

        JwtSecurityTokenHandler handler = new();
        ClaimsPrincipal principal = handler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

        principal.Identity?.IsAuthenticated.Should().BeTrue();
        validatedToken.Should().BeOfType<JwtSecurityToken>();
    }
}

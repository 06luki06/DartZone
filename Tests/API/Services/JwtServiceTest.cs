using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.Tests.API.Services;

[TestFixture]
internal sealed class JwtServiceTest
{
    private const string SecretKey = "SuperSecretJwtKey12345!6789@!012";
    private const string Issuer = "TestIssuer";
    private const string Audience = "TestAudience";

    private IConfiguration _config;
    private JwtService _jwtService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Dictionary<string, string> inMemoryConfig = new()
        {
            {"Jwt:Key", SecretKey},
            {"Jwt:Issuer",  Issuer},
            {"Jwt:Audience", Audience}
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemoryConfig)
            .Build();
    }

    [SetUp]
    public void SetUp()
    {
        _jwtService = new JwtService(_config);
    }

    [Test]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        User user = new("TestUser", [], []);
        string token = _jwtService.GenerateToken(user);

        token.Should().NotBeNullOrEmpty();

        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken jwt = handler.ReadJwtToken(token);

        jwt.Issuer.Should().Be(Issuer);
        jwt.Audiences.Should().Contain(Audience);
    }

    [Test]
    public void GenerateToken_ShouldBeValidAccordingToValidationParameters()
    {
        User user = new("ValidUser", [], []);
        string token = _jwtService.GenerateToken(user);

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

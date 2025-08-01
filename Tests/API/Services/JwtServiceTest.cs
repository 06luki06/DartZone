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

namespace At.luki0606.DartZone.Tests.API.Services
{
    [TestFixture]
    public class JwtServiceTest
    {
        private IConfiguration _config;
        private const string SecretKey = "SuperSecretJwtKey12345!6789@!012";
        private const string Issuer = "TestIssuer";

        [SetUp]
        public void SetUp()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", SecretKey },
                { "Jwt:Issuer", Issuer }
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }

        [Test]
        public void GenerateToken_ShouldReturnValidJwt()
        {
            User user = new("TestUser", [], []);
            string token = JwtService.GenerateToken(user, _config);

            token.Should().NotBeNullOrEmpty();

            JwtSecurityTokenHandler handler = new();
            JwtSecurityToken jwt = handler.ReadJwtToken(token);

            jwt.Issuer.Should().Be(Issuer);
        }

        [Test]
        public void GenerateToken_ShouldBeValidAccordingToValidationParameters()
        {
            User user = new("ValidUser", [], []);
            string token = JwtService.GenerateToken(user, _config);

            byte[] key = Encoding.UTF8.GetBytes(SecretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler handler = new();
            ClaimsPrincipal principal = handler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

            principal.Identity?.IsAuthenticated.Should().BeTrue();
            validatedToken.Should().BeOfType<JwtSecurityToken>();
        }
    }
}

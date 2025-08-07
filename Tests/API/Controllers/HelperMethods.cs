using System;
using System.Collections.Generic;
using System.Security.Claims;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace At.luki0606.DartZone.Tests.API.Controllers
{
    public static class HelperMethods
    {
        internal static ControllerContext CreateControllerContext(User user)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username)
            ];

            ClaimsIdentity identity = new(claims, "TestAuth");
            ClaimsPrincipal principal = new(identity);

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
        }

        internal static DartZoneDbContext CreateDbContext()
        {
            SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            DbContextOptions<DartZoneDbContext> options = new DbContextOptionsBuilder<DartZoneDbContext>()
                .UseSqlite(connection)
                .Options;

            DartZoneDbContext context = new(options);
            context.Database.EnsureCreated();
            return context;
        }

        internal static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IValidator<UserRequestDto>, UserRequestDtoValidator>();
            services.AddScoped<IValidator<DartRequestDto>, DartRequestDtoValidator>();
            services.AddScoped<IValidator<ThrowRequestDto>, ThrowRequestDtoValidator>();

            services.AddScoped<IDtoMapper<User, UserResponseDto>, UserResponseDtoMapper>();
            services.AddScoped<IDtoMapper<Dart, DartResponseDto>, DartResponseDtoMapper>();
            services.AddScoped<IDtoMapper<Throw, ThrowResponseDto>, ThrowResponseDtoMapper>();
            services.AddScoped<IDtoMapper<Game, GameResponseDto>, GameResponseDtoMapper>();

            return services.BuildServiceProvider();
        }
    }
}

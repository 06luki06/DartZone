using System;
using System.Linq;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Services;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.API;

internal sealed class Program
{
    private Program()
    {
    }

    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        AddDbContext(builder);
        AddAuthentication(builder);
        AddValidators(builder);
        AddDtoMappers(builder);
        AddServices(builder);

        builder.Services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                ControllerFeatureProvider oldProvider = manager.FeatureProviders.OfType<ControllerFeatureProvider>().FirstOrDefault();
                if (oldProvider != null)
                {
                    manager.FeatureProviders.Remove(oldProvider);
                }
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DartZoneDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
    }

    private static void AddAuthentication(WebApplicationBuilder builder)
    {
        string key = builder.Configuration["Jwt:Key"];
        byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });

        builder.Services.AddAuthorization();
    }

    private static void AddValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IValidator<UserRequestDto>, UserRequestDtoValidator>();
        builder.Services.AddScoped<IValidator<DartRequestDto>, DartRequestDtoValidator>();
        builder.Services.AddScoped<IValidator<ThrowRequestDto>, ThrowRequestDtoValidator>();

        builder.Services.AddScoped<Validators.IValidatorFactory, ValidatorFactory>();
    }

    private static void AddDtoMappers(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDtoMapper<User, UserResponseDto>, UserResponseDtoMapper>();
        builder.Services.AddScoped<IDtoMapper<Dart, DartResponseDto>, DartResponseDtoMapper>();
        builder.Services.AddScoped<IDtoMapper<Throw, ThrowResponseDto>, ThrowResponseDtoMapper>();
        builder.Services.AddScoped<IDtoMapper<Game, GameResponseDto>, GameResponseDtoMapper>();

        builder.Services.AddScoped<IDtoMapperFactory, DtoMapperFactory>();
    }

    private static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtService, JwtService>();
    }
}

internal class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    protected override bool IsController(System.Reflection.TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass || typeInfo.IsAbstract || typeInfo.ContainsGenericParameters)
        {
            return false;
        }

        return typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) ||
               typeInfo.IsDefined(typeof(Microsoft.AspNetCore.Mvc.ApiControllerAttribute), inherit: true);
    }
}
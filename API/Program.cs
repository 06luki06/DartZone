using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Mappers.Concrete;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.API.Validators;
using At.luki0606.DartZone.API.Validators.Concrete;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using At.luki0606.DartZone.Shared.Dtos.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace At.luki0606.DartZone.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            AddDbContext(builder);
            AddAuthentication(builder);
            AddValidators(builder);
            AddDtoMappers(builder);

            builder.Services.AddControllers();
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
            ConfigurationManager config = builder.Configuration;

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["Jwt:Key"]))
                    };
                });

            builder.Services.AddAuthorization();
        }

        private static void AddValidators(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<UserRequestDto>, UserRequestDtoValidator>();

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
    }
}
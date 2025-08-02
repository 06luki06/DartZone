using System;
using System.Security.Claims;
using System.Threading.Tasks;
using At.luki0606.DartZone.API.Data;
using At.luki0606.DartZone.API.Mappers;
using At.luki0606.DartZone.API.Models;
using At.luki0606.DartZone.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace At.luki0606.DartZone.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly DartZoneDbContext _db;
        protected readonly IDtoMapperFactory _dtoMapperFactory;

        protected BaseController(DartZoneDbContext db, IDtoMapperFactory dtoMapperFactory)
        {
            _db = db;
            _dtoMapperFactory = dtoMapperFactory;
        }

        protected async Task<Result<User>> GetAuthenticatedUser()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Result<User>.Failure("User not authenticated.");
            }

            if (!Guid.TryParse(userId, out Guid parsedUserId))
            {
                return Result<User>.Failure("Invalid user identifier.");
            }

            User user = await _db.Users.FindAsync(parsedUserId);
            if (user == null)
            {
                return Result<User>.Failure("User not found.");
            }

            return Result<User>.Success(user);
        }
    }
}

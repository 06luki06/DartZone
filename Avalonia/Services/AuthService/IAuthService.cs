using System.Threading.Tasks;
using At.luki0606.DartZone.Shared.Dtos.Requests;

namespace At.luki0606.DartZone.AvaloniaUI.Services.AuthService;

internal interface IAuthService
{
    Task<bool> LoginAsync(UserRequestDto dto);
    Task<bool> RegisterAsync(UserRequestDto dto);
}

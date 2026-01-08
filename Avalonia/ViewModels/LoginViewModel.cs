using System.Threading.Tasks;
using At.luki0606.DartZone.AvaloniaUI.Services.AuthService;
using At.luki0606.DartZone.AvaloniaUI.Utils;
using At.luki0606.DartZone.AvaloniaUI.Views;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels;

internal sealed class LoginViewModel : AAuthViewModel
{
    public LoginViewModel(IRegionManager regionManager, IAuthService authService)
        : base(regionManager, nameof(RegisterView), authService)
    {
    }

    protected override async Task OnSubmit()
    {
        UserRequestDto dto = new()
        {
            Username = Username,
            Password = Password
        };

        bool success = await AuthService.LoginAsync(dto).ConfigureAwait(true);

        if (success)
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, nameof(GamesView));
        }
        else
        {
            // Handle login failure (e.g., show error message)
        }
    }
}

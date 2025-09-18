using System.Threading.Tasks;
using At.luki0606.DartZone.AvaloniaUI.Services.AuthService;
using At.luki0606.DartZone.AvaloniaUI.Utils;
using At.luki0606.DartZone.AvaloniaUI.Views;
using At.luki0606.DartZone.Shared.Dtos.Requests;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels
{
    public class RegisterViewModel : AAuthViewModel
    {
        public RegisterViewModel(IRegionManager regionManager, IAuthService authService)
            : base(regionManager, nameof(LoginView), authService)
        {
        }

        protected override async Task OnSubmit()
        {
            UserRequestDto dto = new()
            {
                Username = Username,
                Password = Password
            };

            bool success = await AuthService.RegisterAsync(dto);

            if (success)
            {
                RegionManager.RequestNavigate(RegionNames.CONTENT_REGION, nameof(GamesView));
            }
            else
            {
                // Handle login failure (e.g., show error message)
            }
        }
    }
}

using System.Threading.Tasks;
using At.luki0606.DartZone.AvaloniaUI.Services.AuthService;
using At.luki0606.DartZone.AvaloniaUI.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels
{
    public abstract class AAuthViewModel : BindableBase
    {
        #region Properties
        public readonly IRegionManager RegionManager;
        public readonly IAuthService AuthService;

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        #region Commands
        public DelegateCommand NavigateCmd { get; }
        public DelegateCommand SubmitCmd { get; }
        #endregion

        protected AAuthViewModel(IRegionManager regionManager, string navigationTarget, IAuthService authService)
        {
            RegionManager = regionManager;
            AuthService = authService;
            NavigateCmd = new(() =>
            {
                RegionManager.RequestNavigate(RegionNames.CONTENT_REGION, navigationTarget);
            });
            SubmitCmd = new(async () => await OnSubmit());
        }

        protected abstract Task OnSubmit();
    }
}

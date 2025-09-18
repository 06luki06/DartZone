using At.luki0606.DartZone.AvaloniaUI.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels
{
    public abstract class AAuthViewModel : BindableBase
    {
        #region Fields
        private readonly IRegionManager _regionManager;
        #endregion

        #region Properties
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

        protected AAuthViewModel(IRegionManager regionManager, string navigationTarget)
        {
            _regionManager = regionManager;
            NavigateCmd = new(() =>
            {
                _regionManager.RequestNavigate(RegionNames.CONTENT_REGION, navigationTarget);
            });
            SubmitCmd = new(OnSubmit);
        }

        protected abstract void OnSubmit();
    }
}

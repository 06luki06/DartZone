using At.luki0606.DartZone.AvaloniaUI.Views;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels
{
    public class LoginViewModel : AAuthViewModel
    {
        public LoginViewModel(IRegionManager regionManager)
            : base(regionManager, nameof(RegisterView))
        {
        }

        protected override void OnSubmit()
        {
            throw new System.NotImplementedException();
        }
    }
}

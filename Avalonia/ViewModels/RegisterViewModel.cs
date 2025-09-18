using At.luki0606.DartZone.AvaloniaUI.Views;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels
{
    public class RegisterViewModel : AAuthViewModel
    {
        public RegisterViewModel(IRegionManager regionManager)
            : base(regionManager, nameof(LoginView))
        {
        }

        protected override void OnSubmit()
        {
            throw new System.NotImplementedException();
        }
    }
}

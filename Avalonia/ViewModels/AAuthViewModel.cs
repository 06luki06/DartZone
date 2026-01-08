using System.Threading.Tasks;
using At.luki0606.DartZone.AvaloniaUI.Services.AuthService;
using At.luki0606.DartZone.AvaloniaUI.Utils;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI.ViewModels;

internal abstract class AAuthViewModel : BindableBase
{
    #region Properties
    public readonly IRegionManager RegionManager;
    public readonly IAuthService AuthService;

    public string Username
    {
        get;
        set => SetProperty(ref field, value);
    }
    public string Password
    {
        get;
        set => SetProperty(ref field, value);
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
            RegionManager.RequestNavigate(RegionNames.ContentRegion, navigationTarget);
        });
        SubmitCmd = new(async () => await OnSubmit().ConfigureAwait(false));
    }

    protected abstract Task OnSubmit();
}

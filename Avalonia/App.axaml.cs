using System.Net.Http;
using At.luki0606.DartZone.AvaloniaUI.Services.AuthService;
using At.luki0606.DartZone.AvaloniaUI.Utils;
using At.luki0606.DartZone.AvaloniaUI.Views;
using Avalonia;
using Avalonia.Markup.Xaml;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI;

internal sealed partial class App : PrismApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }

    protected override AvaloniaObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<HttpClient>(() => new HttpClient
        {
#pragma warning disable S1075
            BaseAddress = new System.Uri("http://localhost:5000/api/"),
#pragma warning restore S1075
            Timeout = System.TimeSpan.FromSeconds(30)
        });

        containerRegistry.Register<IAuthService, AuthService>();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        IRegionManager regionManager = this.Container.Resolve<IRegionManager>();

        regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(LoginView));
        regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(RegisterView));
        regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(GamesView));

        regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(LoginView));
    }
}
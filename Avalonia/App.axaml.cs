using At.luki0606.DartZone.AvaloniaUI.Utils;
using At.luki0606.DartZone.AvaloniaUI.Views;
using Avalonia;
using Avalonia.Markup.Xaml;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Navigation.Regions;

namespace At.luki0606.DartZone.AvaloniaUI
{
    public partial class App : PrismApplication
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
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            IRegionManager regionManager = this.Container.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(LoginView));
            regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(RegisterView));
            regionManager.RegisterViewWithRegion(RegionNames.CONTENT_REGION, typeof(GamesView));

            regionManager.RequestNavigate(RegionNames.CONTENT_REGION, nameof(LoginView));
        }
    }
}
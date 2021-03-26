using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Navigation.Menu.iOS.Navigation;
using AppRopio.Navigation.Menu.iOS.Services;
using AppRopio.Navigation.Menu.iOS.Services.Implementation;
using AppRopio.Navigation.Menu.iOS.Views;
using MvvmCross;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.ViewModels;

namespace AppRopio.Navigation.Menu.iOS
{
    public class MenuSetup : BaseIosSetup
    {
        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxIosViewPresenter CreateViewPresenter()
        {
            return new MenuNavigationPresenter(ApplicationDelegate, Window);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterSingleton<IMenuThemeConfigService>(() => new MenuThemeConfigService());

            Mvx.IoCProvider.Resolve<IViewLookupService>().Register<IMenuViewModel, MenuViewController>();
        }
    }
}

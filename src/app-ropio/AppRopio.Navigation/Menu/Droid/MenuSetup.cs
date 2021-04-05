using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Navigation.Menu.Droid.Navigation;
using AppRopio.Navigation.Menu.Droid.Services;
using AppRopio.Navigation.Menu.Droid.Views;
using MvvmCross;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;

namespace AppRopio.Navigation.Menu.Droid
{
    public class MenuSetup : BaseAndroidSetup
    {
        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxAndroidViewPresenter CreateViewPresenter()
        {
            return new DrawerAndroidViewPresenter(AndroidViewAssemblies);
        }

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            Mvx.IoCProvider.RegisterSingleton<IMenuThemeConfigService>(() => new MenuThemeConfigService());

            Mvx.IoCProvider.Resolve<IViewLookupService>().Register<IMenuViewModel, MenuActivity>();
        }
    }
}

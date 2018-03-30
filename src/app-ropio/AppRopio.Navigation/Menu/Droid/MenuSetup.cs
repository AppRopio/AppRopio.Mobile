using Android.Content;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Navigation.Menu.Droid.Navigation;
using AppRopio.Navigation.Menu.Droid.Services;
using AppRopio.Navigation.Menu.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace AppRopio.Navigation.Menu.Droid
{
    public class MenuSetup : BaseAndroidSetup
    {
        protected MenuSetup(Context applicationContext)
            : base(applicationContext)
        {
            
        }

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

            Mvx.RegisterSingleton<IMenuThemeConfigService>(() => new MenuThemeConfigService());

            Mvx.Resolve<IViewLookupService>().Register<IMenuViewModel, MenuActivity>();
        }
    }
}

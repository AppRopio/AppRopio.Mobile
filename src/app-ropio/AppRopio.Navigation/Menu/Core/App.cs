using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Navigation.Menu.Core.Services;
using AppRopio.Navigation.Menu.Core.Services.Implementation;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Navigation.Menu.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Logging;

namespace AppRopio.Navigation.Menu.Core
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IMenuConfigService>(() => new MenuConfigService());
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMenuVmService, MenuVmService>();

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

            vmLookupService.Register<IMenuViewModel>(typeof(MenuViewModel));

            #endregion

            RegisterAppStart<ViewModels.MenuViewModel>();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Menu module is loaded");
        }
    }
}

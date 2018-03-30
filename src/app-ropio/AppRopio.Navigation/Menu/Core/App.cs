using AppRopio.Navigation.Menu.Core.Services;
using AppRopio.Navigation.Menu.Core.Services.Implementation;
using AppRopio.Navigation.Menu.Core.ViewModels.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using AppRopio.Navigation.Menu.Core.ViewModels;
using AppRopio.Base.Core.Services.ViewModelLookup;

namespace AppRopio.Navigation.Menu.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<IMenuConfigService>(() => new MenuConfigService());
            Mvx.RegisterSingleton<IMenuVmService>(() => new MenuVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

            vmLookupService.Register<IMenuViewModel>(typeof(MenuViewModel));

            #endregion

            RegisterAppStart<ViewModels.MenuViewModel>();

            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Menu module is loaded");
        }
    }
}

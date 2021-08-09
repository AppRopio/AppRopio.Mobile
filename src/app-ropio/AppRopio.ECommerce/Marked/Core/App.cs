using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.Marked.Core.Services;
using AppRopio.ECommerce.Marked.Core.Services.Implementation;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Services;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Marked.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
			new API.App().Initialize();

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.IoCProvider.RegisterSingleton<IMarkedObservableService>(new MarkedObservableService()));

            Mvx.IoCProvider.RegisterSingleton<IMarkedConfigService>(() => new MarkedConfigService());

            Mvx.IoCProvider.RegisterSingleton<IMarkedVmService>(() => new MarkedVmService());

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

            vmLookupService.Register<IMarkedViewModel, MarkedViewModel>();

            #endregion

            //register start point for current navigation module
            var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
            routerService.Register<IMarkedViewModel>(new MarkedRouterSubscriber());
        }
    }
}
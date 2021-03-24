using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.Marked.Core.Services;
using AppRopio.ECommerce.Marked.Core.Services.Implementation;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Services;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Marked.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.RegisterSingleton<IMarkedObservableService>(new MarkedObservableService()));

            Mvx.RegisterSingleton<IMarkedConfigService>(() => new MarkedConfigService());

            Mvx.RegisterSingleton<IMarkedVmService>(() => new MarkedVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

            vmLookupService.Register<IMarkedViewModel, MarkedViewModel>();

            #endregion

            //register start point for current navigation module
            var routerService = Mvx.Resolve<IRouterService>();
            routerService.Register<IMarkedViewModel>(new MarkedRouterSubscriber());
        }
    }
}
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.CloudPayments.Core.Services.Implementation;
using AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.CloudPayments.Core
{
    public class App : MvxApplication
    {
		public override void Initialize()
		{
            new API.App().Initialize();

            new AppRopio.Payments.API.App().Initialize();

			Mvx.IoCProvider.RegisterType<ICloudPaymentsPaymentNavigationVmService>(() => new CloudPaymentsPaymentNavigationVmService());

			Mvx.IoCProvider.RegisterSingleton<ICloudPaymentsConfigService>(() => new CloudPaymentsConfigService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
            routerService.Register<ICloudPaymentsVmService>(new CloudPaymentsRouterSubscriber());
		}
    }
}
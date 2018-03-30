using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.CloudPayments.Core.Services.Implementation;
using AppRopio.Payments.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services;

namespace AppRopio.Payments.CloudPayments.Core
{
    public class App : MvxApplication
    {
		public override void Initialize()
		{
            Mvx.RegisterSingleton<ICloudPaymentsPaymentNavigationVmService>(() => new CloudPaymentsPaymentNavigationVmService());
			Mvx.RegisterSingleton<ICloudPaymentsConfigService>(() => new CloudPaymentsConfigService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

			#endregion

			//register start point for current navigation module
			var routerService = Mvx.Resolve<IRouterService>();
            routerService.Register<ICloudPaymentsVmService>(new CloudPaymentsRouterSubscriber());
		}
    }
}
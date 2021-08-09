using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.Services.Implementation;
using AppRopio.Payments.Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AppRopio.Payments.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            new API.App().Initialize();

			Mvx.IoCProvider.RegisterSingleton<IPaymentsVmService>(() => new PaymentsVmService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
			vmLookupService.Register<ICardPaymentViewModel, CardPaymentViewModel>();

			#endregion
		}
	}
}

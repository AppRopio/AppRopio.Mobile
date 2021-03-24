using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.Services.Implementation;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.Core.ViewModels.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
			Mvx.RegisterSingleton<IPaymentsVmService>(() => new PaymentsVmService());

			#region VMs registration

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
			vmLookupService.Register<ICardPaymentViewModel, CardPaymentViewModel>();

			#endregion
		}
	}
}

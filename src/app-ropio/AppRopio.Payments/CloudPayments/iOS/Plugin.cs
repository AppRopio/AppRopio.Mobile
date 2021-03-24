using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.CloudPayments.iOS.Services.Implementation;
using AppRopio.Payments.Core.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.CloudPayments.iOS
{
    public class Plugin : IMvxPlugin
	{
		public void Load()
		{
            Mvx.RegisterSingleton<INativePaymentService>(() => new NativePaymentService());
            Mvx.RegisterSingleton<IPayment3DSService>(() => new CloudPayments3DSService());

			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			//viewLookupService.Register<ICardPaymentViewModel, CloudPaymentsCardPaymentViewController>();
		}
	}
}
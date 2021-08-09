using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.CloudPayments.Core;
using AppRopio.Payments.CloudPayments.iOS.Services.Implementation;
using AppRopio.Payments.Core.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.CloudPayments.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "CloudPayments";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<INativePaymentService>(() => new NativePaymentService());
            Mvx.IoCProvider.RegisterSingleton<IPayment3DSService>(() => new CloudPayments3DSService());

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			//viewLookupService.Register<ICardPaymentViewModel, CloudPaymentsCardPaymentViewController>();
		}
	}
}
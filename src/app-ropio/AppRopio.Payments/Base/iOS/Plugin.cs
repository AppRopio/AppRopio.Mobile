using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.CloudPayments.iOS.View;
using AppRopio.Payments.Core;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.iOS.Services;
using AppRopio.Payments.iOS.Services.Implementation;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IPaymentsThemeConfigService>(() => new PaymentsThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<ICardPaymentViewModel, CardPaymentViewController>();
        }
    }
}

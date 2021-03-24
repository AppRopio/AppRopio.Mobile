using System;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.CloudPayments.iOS.View;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.iOS.Services;
using AppRopio.Payments.iOS.Services.Implementation;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IPaymentsThemeConfigService>(() => new PaymentsThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<ICardPaymentViewModel, CardPaymentViewController>();
        }
    }
}

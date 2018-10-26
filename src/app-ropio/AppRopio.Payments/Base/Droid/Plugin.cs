using System;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Payments.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<ICardPaymentViewModel>(typeof(CardPaymentFragment));
        }
    }
}

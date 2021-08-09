using System;
using AppRopio.Base.API;
using AppRopio.ECommerce.Loyalty.API.Services;
using AppRopio.ECommerce.Loyalty.API.Services.Fake;
using AppRopio.ECommerce.Loyalty.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.ECommerce.Loyalty.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterSingleton<IPromoService>(() => new PromoFakeService());
            else
                Mvx.IoCProvider.RegisterSingleton<IPromoService>(() => new PromoService());
        }
    }
}

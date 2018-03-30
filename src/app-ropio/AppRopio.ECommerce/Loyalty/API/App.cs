using System;
using AppRopio.Base.API;
using AppRopio.ECommerce.Loyalty.API.Services;
using AppRopio.ECommerce.Loyalty.API.Services.Fake;
using AppRopio.ECommerce.Loyalty.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Loyalty.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterSingleton<IPromoService>(() => new PromoFakeService());
            else
                Mvx.RegisterSingleton<IPromoService>(() => new PromoService());
        }
    }
}

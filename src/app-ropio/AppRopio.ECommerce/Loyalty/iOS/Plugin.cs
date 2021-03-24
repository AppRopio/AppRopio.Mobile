using MvvmCross;
using AppRopio.ECommerce.Loyalty.iOS.Services;
using AppRopio.ECommerce.Loyalty.iOS.Services.Implementation;
using MvvmCross.Plugin;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.iOS.Views.Promo;

namespace AppRopio.ECommerce.Loyalty.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<ILoyaltyThemeConfigService>(() => new LoyaltyThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            viewLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeView));
            viewLookupService.Register<PromoCodeViewModel>(typeof(PromoCodeView)); //для интеграции в другие модули
        }
    }
}

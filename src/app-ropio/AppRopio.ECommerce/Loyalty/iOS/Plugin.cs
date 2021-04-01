using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Loyalty.Core;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.iOS.Services;
using AppRopio.ECommerce.Loyalty.iOS.Services.Implementation;
using AppRopio.ECommerce.Loyalty.iOS.Views.Promo;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Loyalty.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<ILoyaltyThemeConfigService>(() => new LoyaltyThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            viewLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeView));
            viewLookupService.Register<PromoCodeViewModel>(typeof(PromoCodeView)); //для интеграции в другие модули
        }
    }
}

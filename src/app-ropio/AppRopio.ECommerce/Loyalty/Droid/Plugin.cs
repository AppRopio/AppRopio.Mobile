using MvvmCross;
using MvvmCross.Plugin;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Loyalty.Droid.Views.Promo;

namespace AppRopio.ECommerce.Loyalty.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            viewLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeView));
            viewLookupService.Register<PromoCodeViewModel>(typeof(PromoCodeView)); //для интеграции в другие модули
        }
    }
}

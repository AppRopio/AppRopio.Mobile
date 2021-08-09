using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Loyalty.Core;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.Droid.Views.Promo;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Loyalty.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Loyalty";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            viewLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeView));
            viewLookupService.Register<PromoCodeViewModel>(typeof(PromoCodeView)); //для интеграции в другие модули
        }
    }
}

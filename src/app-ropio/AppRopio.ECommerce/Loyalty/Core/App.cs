using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.Platform;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo.Services;
using AppRopio.ECommerce.Loyalty.Core.Services;
using AppRopio.ECommerce.Loyalty.Core.Services.Implementation;

namespace AppRopio.ECommerce.Loyalty.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<ILoyaltyConfigService>(() => new LoyaltyConfigService());
            Mvx.RegisterSingleton<IPromoVmService>(() => new PromoVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeViewModel));

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.Resolve<IRouterService>();

            #endregion
        }
    }
}

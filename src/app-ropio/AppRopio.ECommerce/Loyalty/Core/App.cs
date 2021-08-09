using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo.Services;
using AppRopio.ECommerce.Loyalty.Core.Services;
using AppRopio.ECommerce.Loyalty.Core.Services.Implementation;

namespace AppRopio.ECommerce.Loyalty.Core
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            new API.App().Initialize();

            Mvx.IoCProvider.RegisterSingleton<ILoyaltyConfigService>(() => new LoyaltyConfigService());
            Mvx.IoCProvider.RegisterSingleton<IPromoVmService>(() => new PromoVmService());

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IPromoCodeViewModel>(typeof(PromoCodeViewModel));

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.IoCProvider.Resolve<IRouterService>();

            #endregion
        }
    }
}

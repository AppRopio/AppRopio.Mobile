using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.Services.Implementation;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services.Implementation;
using MvvmCross.Platform;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using MvvmCross.Plugins.Messenger;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;

namespace AppRopio.ECommerce.Basket.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.RegisterSingleton<IBasketObservableService>(new BasketObservableService()));

            Mvx.RegisterSingleton<IBasketConfigService>(() => new BasketConfigService());
            Mvx.RegisterSingleton<IBasketItemVmService>(() => new BasketItemVmService());
            Mvx.RegisterSingleton<IBasketVmService>(() => new BasketVmService());
            Mvx.RegisterSingleton<IOrderVmService>(() => new OrderVmService());
            Mvx.RegisterSingleton<IUserVmService>(() => new UserVmService());
            Mvx.RegisterSingleton<IDeliveryVmService>(() => new DeliveryVmService());
            Mvx.RegisterSingleton<IBasketNavigationVmService>(() => new BasketNavigationVmService());

            Mvx.LazyConstructAndRegisterSingleton<IBasketProductCardVmService, BasketProductCardVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IProductQuantityVmService, ProductQuantityVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IProductDeleteVmService, ProductDeleteVmService>();

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IBasketViewModel, BasketViewModel>();
            vmLookupService.Register<IDeliveryViewModel, DeliveryViewModel>();
            vmLookupService.Register<IDeliveryOnPointVM, DeliveryOnPointVM>();
            vmLookupService.Register<IDeliveryOnAddressVM, DeliveryOnAddressVM>();
            vmLookupService.Register<IDeliveryPointOnMapVM, DeliveryPointOnMapVM>();
            vmLookupService.Register<IDeliveryPointAdditionalInfoVM, DeliveryPointAdditionalInfoVM>();
            vmLookupService.Register<IUserViewModel, UserViewModel>();
            vmLookupService.Register<IPaymentViewModel, PaymentViewModel>();
            vmLookupService.Register<IBasketProductCardViewModel, BasketProductCardViewModel>();
            vmLookupService.Register<IBasketCartIndicatorViewModel, BasketCartIndicatorViewModel>();
            vmLookupService.Register<IThanksViewModel, ThanksViewModel>();
            vmLookupService.Register<IOrderFieldAutocompleteVM, OrderFieldAutocompleteVM>();

            var orderType = Mvx.Resolve<IBasketConfigService>().Config.OrderViewType;
            switch (orderType)
            {
                case Enums.OrderViewType.Full:
                    vmLookupService.Register<IOrderViewModel, FullOrderViewModel>();
                    break;
                case Enums.OrderViewType.Partial:
                    vmLookupService.Register<IOrderViewModel, UserViewModel>();
                    break;
            }


            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.Resolve<IRouterService>();

            routerService.Register<IBasketViewModel>(new BasketRouterSubscriber());

            #endregion
        }
    }
}

using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.Services.Implementation;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;
using AppRopio.ECommerce.Basket.Core.ViewModels.CatalogCard;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services.Implementation;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard.Services;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            new API.App().Initialize();

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.IoCProvider.RegisterSingleton<IBasketObservableService>(new BasketObservableService()));

            Mvx.IoCProvider.RegisterSingleton<IBasketConfigService>(() => new BasketConfigService());
            Mvx.IoCProvider.RegisterSingleton<IBasketItemVmService>(() => new BasketItemVmService());
            Mvx.IoCProvider.RegisterSingleton<IBasketVmService>(() => new BasketVmService());
            Mvx.IoCProvider.RegisterSingleton<IOrderVmService>(() => new OrderVmService());
            Mvx.IoCProvider.RegisterSingleton<IUserVmService>(() => new UserVmService());
            Mvx.IoCProvider.RegisterSingleton<IDeliveryVmService>(() => new DeliveryVmService());
            Mvx.IoCProvider.RegisterSingleton<IBasketNavigationVmService>(() => new BasketNavigationVmService());

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBasketProductCardVmService, BasketProductCardVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductQuantityVmService, ProductQuantityVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductDeleteVmService, ProductDeleteVmService>();

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IBasketViewModel, BasketViewModel>();
            vmLookupService.Register<IDeliveryViewModel, DeliveryViewModel>();
            vmLookupService.Register<IDeliveryOnPointVM, DeliveryOnPointVM>();
            vmLookupService.Register<IDeliveryOnAddressVM, DeliveryOnAddressVM>();
            vmLookupService.Register<IDeliveryPointOnMapVM, DeliveryPointOnMapVM>();
            vmLookupService.Register<IDeliveryPointAdditionalInfoVM, DeliveryPointAdditionalInfoVM>();
            vmLookupService.Register<IUserViewModel, UserViewModel>();
            vmLookupService.Register<IPaymentViewModel, PaymentViewModel>();
            vmLookupService.Register<IBasketProductCardViewModel, BasketProductCardViewModel>();
            vmLookupService.Register<IBasketCatalogItemVM, BasketCatalogItemVM>();
            vmLookupService.Register<IBasketCartIndicatorViewModel, BasketCartIndicatorViewModel>();
            vmLookupService.Register<IThanksViewModel, ThanksViewModel>();
            vmLookupService.Register<IOrderFieldAutocompleteVM, OrderFieldAutocompleteVM>();

            var orderType = Mvx.IoCProvider.Resolve<IBasketConfigService>().Config.OrderViewType;
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

            var routerService = Mvx.IoCProvider.Resolve<IRouterService>();

            routerService.Register<IBasketViewModel>(new BasketRouterSubscriber());

            #endregion
        }
    }
}

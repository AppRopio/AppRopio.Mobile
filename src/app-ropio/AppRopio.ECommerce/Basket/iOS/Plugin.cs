using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Services.Implementation;
using AppRopio.ECommerce.Basket.iOS.Views.Basket;
using AppRopio.ECommerce.Basket.iOS.Views.CartIndicator;
using AppRopio.ECommerce.Basket.iOS.Views.Order;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Full;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Partial;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Payment;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Thanks;
using AppRopio.ECommerce.Basket.iOS.Views.ProductCard;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Basket.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IBasketThemeConfigService>(() => new BasketThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            viewLookupService.Register<IBasketViewModel>(typeof(BasketViewController));
            viewLookupService.Register<IDeliveryViewModel>(typeof(DeliveryTypesViewController));
            viewLookupService.Register<IDeliveryOnPointVM>(typeof(DeliveryOnPointVC));
            viewLookupService.Register<IDeliveryOnAddressVM>(typeof(DeliveryOnAddressVC));
            viewLookupService.Register<IDeliveryPointOnMapVM>(typeof(DeliveryPointOnMapVC));
            viewLookupService.Register<IDeliveryPointAdditionalInfoVM>(typeof(DeliveryPointAdditionalInfoVC));
            viewLookupService.Register<IPaymentViewModel>(typeof(PaymentViewController));
            viewLookupService.Register<IThanksViewModel>(typeof(ThanksOrderViewController));
            viewLookupService.Register<IOrderFieldAutocompleteVM>(typeof(OrderFieldAutocompleteViewController));

            viewLookupService.Register<IBasketProductCardViewModel, BasketProductCardView>();
            viewLookupService.Register<IBasketCartIndicatorViewModel, BasketCartIndicatorView>();

            var orderType = Mvx.Resolve<IBasketConfigService>().Config.OrderViewType;
            switch (orderType)
            {
                case Core.Enums.OrderViewType.Full:
                    viewLookupService.Register<IOrderViewModel>(typeof(FullOrderViewController));
                    break;
                case Core.Enums.OrderViewType.Partial:
                    viewLookupService.Register<IOrderViewModel>(typeof(UserViewController));
                    break;
            }
        }
    }
}

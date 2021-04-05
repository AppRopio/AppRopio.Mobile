using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Basket.Droid.Views.Basket;
using AppRopio.ECommerce.Basket.Droid.Views.CartIndicator;
using AppRopio.ECommerce.Basket.Droid.Views.Order.Delivery;
using AppRopio.ECommerce.Basket.Droid.Views.Order.Full;
using AppRopio.ECommerce.Basket.Droid.Views.Order.Payment;
using AppRopio.ECommerce.Basket.Droid.Views.Order.Thanks;
using AppRopio.ECommerce.Basket.Droid.Views.ProductCard;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Basket.Droid {
	[MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            viewLookupService.Register<IBasketViewModel, BasketFragment>();
            //viewLookupService.Register<IDeliveryViewModel>(typeof(DeliveryTypesViewController));
            viewLookupService.Register<IDeliveryOnAddressVM>(typeof(DeliveryOnAddressFragment));
			viewLookupService.Register<IDeliveryOnPointVM>(typeof(DeliveryOnPointFragment));
            viewLookupService.Register<IDeliveryPointOnMapVM>(typeof(DeliveryPointOnMapFragment));
            viewLookupService.Register<IDeliveryPointAdditionalInfoVM>(typeof(DeliveryPointAdditionalInfoFragment));
            viewLookupService.Register<IPaymentViewModel>(typeof(PaymentFragment));
            viewLookupService.Register<IThanksViewModel>(typeof(ThanksFragment));

            viewLookupService.Register<IBasketProductCardViewModel, BasketProductCardView>();
            viewLookupService.Register<IBasketCartIndicatorViewModel, BasketCartIndicatorView>();

            viewLookupService.Register<IOrderViewModel>(typeof(FullOrderFragment));
        }
    }
}

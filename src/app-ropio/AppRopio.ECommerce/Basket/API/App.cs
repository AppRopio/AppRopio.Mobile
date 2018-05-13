using AppRopio.Base.API;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.API.Services.Fake;
using AppRopio.ECommerce.Basket.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Basket.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
            {
                Mvx.RegisterType<IBasketService>(() => new BasketFakeService());
                Mvx.RegisterType<IOrderService>(() => new OrderFakeService());
                Mvx.RegisterType<IDeliveryService>(() => new DeliveryFakeService());
            }
            else
            {
                Mvx.RegisterType<IBasketService>(() => new BasketService());
                Mvx.RegisterType<IOrderService>(() => new OrderService());
                Mvx.RegisterType<IDeliveryService>(() => new DeliveryService());
            }
        }
    }
}

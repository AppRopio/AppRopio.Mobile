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
                Mvx.RegisterSingleton<IBasketService>(() => new BasketFakeService());
                Mvx.RegisterSingleton<IOrderService>(() => new OrderFakeService());
                Mvx.RegisterSingleton<IDeliveryService>(() => new DeliveryFakeService());
            }
            else
            {
                Mvx.RegisterSingleton<IBasketService>(() => new BasketService());
                Mvx.RegisterSingleton<IOrderService>(() => new OrderService());
                Mvx.RegisterSingleton<IDeliveryService>(() => new DeliveryService());
            }
        }
    }
}

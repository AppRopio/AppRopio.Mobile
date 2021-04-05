using AppRopio.Base.API;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.API.Services.Fake;
using AppRopio.ECommerce.Basket.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
            {
                Mvx.IoCProvider.RegisterType<IBasketService>(() => new BasketFakeService());
                Mvx.IoCProvider.RegisterType<IOrderService>(() => new OrderFakeService());
                Mvx.IoCProvider.RegisterType<IDeliveryService>(() => new DeliveryFakeService());
            }
            else
            {
                Mvx.IoCProvider.RegisterType<IBasketService>(() => new BasketService());
                Mvx.IoCProvider.RegisterType<IOrderService>(() => new OrderService());
                Mvx.IoCProvider.RegisterType<IDeliveryService>(() => new DeliveryService());
            }
        }
    }
}

using System;
using AppRopio.Base.API;
using AppRopio.ECommerce.HistoryOrders.API.Services;
using AppRopio.ECommerce.HistoryOrders.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<IHistoryOrdersService>(() => new AppRopio.ECommerce.HistoryOrders.API.Services.Fakes.HistoryOrdersFakeService());
            else
                Mvx.RegisterType<IHistoryOrdersService>(() => new HistoryOrdersService());
        }
    }
}

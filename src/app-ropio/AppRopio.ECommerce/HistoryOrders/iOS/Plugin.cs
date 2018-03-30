using AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using AppRopio.ECommerce.HistoryOrders.iOS.Services.Implementation;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;

namespace AppRopio.ECommerce.HistoryOrders.iOS
{
    public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			Mvx.RegisterSingleton<IHistoryOrdersThemeConfigService>(() => new HistoryOrdersThemeConfigService());

			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			viewLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersViewController>();
            viewLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsViewController>();
            viewLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsViewController>();
		}
	}
}

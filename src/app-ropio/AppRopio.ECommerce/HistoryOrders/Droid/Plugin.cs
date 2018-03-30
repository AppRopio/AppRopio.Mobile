using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderDetails;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderProducts;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrders;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.ECommerce.HistoryOrders.Droid
{
    public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			viewLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersFragment>();
            viewLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsFragment>();
            viewLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsFragment>();
		}
	}
}

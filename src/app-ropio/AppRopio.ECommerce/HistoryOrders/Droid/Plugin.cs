using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.HistoryOrders.Core;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderDetails;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderProducts;
using AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrders;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.HistoryOrders.Droid
{
	[MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			viewLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersFragment>();
            viewLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsFragment>();
            viewLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsFragment>();
		}
	}
}

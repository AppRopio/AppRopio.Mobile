using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.HistoryOrders.Core;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using AppRopio.ECommerce.HistoryOrders.iOS.Services.Implementation;
using AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.HistoryOrders.iOS
{
	[MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

			Mvx.IoCProvider.RegisterSingleton<IHistoryOrdersThemeConfigService>(() => new HistoryOrdersThemeConfigService());

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			viewLookupService.Register<IHistoryOrdersViewModel, HistoryOrdersViewController>();
            viewLookupService.Register<IHistoryOrderDetailsViewModel, HistoryOrderDetailsViewController>();
            viewLookupService.Register<IHistoryOrderProductsViewModel, HistoryOrderProductsViewController>();
		}
	}
}

using System.Reflection;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.Core.Services.Implementation
{
    public class HistoryOrdersNavigationVmService : BaseVmNavigationService, IHistoryOrdersNavigationVmService
    {
        public void NavigateToOrder(HistoryOrderBundle bundle)
        {
            NavigateTo<IHistoryOrderDetailsViewModel>(bundle);
        }

		public void NavigateToOrderProducts(HistoryOrderBundle bundle)
		{
			NavigateTo<IHistoryOrderProductsViewModel>(bundle);
		}

		public void NavigateToBasket(BaseBundle bundle)
		{
            var config = Mvx.Resolve<IHistoryOrdersConfigService>().Config;
			if (config.Basket != null)
			{
				var assembly = Assembly.Load(new AssemblyName(config.Basket.AssemblyName));

				var basketType = assembly.GetType(config.Basket.TypeName);

				NavigateTo(basketType, bundle);
			}
		}
    }
}
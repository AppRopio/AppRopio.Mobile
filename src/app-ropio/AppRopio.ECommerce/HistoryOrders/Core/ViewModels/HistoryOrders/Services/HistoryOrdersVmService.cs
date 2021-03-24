using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.HistoryOrders.API.Services;
using MvvmCross.ViewModels;
using MvvmCross;
using AppRopio.Base.API.Exceptions;
using AppRopio.ECommerce.HistoryOrders.Core.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using System.Linq;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services
{
    public class HistoryOrdersVmService : BaseVmService, IHistoryOrdersVmService
    {
		#region Services

		protected IHistoryOrdersService ApiService { get { return Mvx.Resolve<IHistoryOrdersService>(); } }

		#endregion
		
        public async Task<MvxObservableCollection<IHistoryOrderItemVM>> LoadHistoryOrders(int count, int offset = 0)
        {
            MvxObservableCollection<IHistoryOrderItemVM> dataSource = null;

			try
			{
                var orders = await ApiService.GetOrders(count, offset);

                dataSource = new MvxObservableCollection<IHistoryOrderItemVM>(orders.Select(o => new HistoryOrderItemVM(o)));
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return dataSource;
        }

        public void HandleOrderSelection(IHistoryOrderItemVM item)
		{
			var historyOrdersNavigationVmService = Mvx.Resolve<IHistoryOrdersNavigationVmService>();
            var orderBundle = new HistoryOrderBundle(item.OrderId, item.OrderNumber, item.TotalPrice, item.ItemsCount, NavigationType.Push);
            historyOrdersNavigationVmService.NavigateToOrder(orderBundle);
		}
	}
}
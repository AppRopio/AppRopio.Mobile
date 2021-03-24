using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.HistoryOrders.API.Services;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;
using AppRopio.ECommerce.HistoryOrders.Core.Services;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services
{
    public class HistoryOrderDetailsVmService : BaseVmService, IHistoryOrderDetailsVmService
    {
		#region Services

		protected IHistoryOrdersService ApiService { get { return Mvx.Resolve<IHistoryOrdersService>(); } }

		#endregion
		
        public async Task<HistoryOrderDetails> LoadOrderDetails(string orderId)
        {
            HistoryOrderDetails details = null;

			try
			{
                details = await ApiService.GetOrderDetails(orderId);
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

            return details;
        }

		public async Task<MvxObservableCollection<IHistoryOrderProductItemVM>> LoadOrderProducts(string orderId)
		{
            MvxObservableCollection<IHistoryOrderProductItemVM> dataSource = null;

			try
			{
                var products = await ApiService.GetOrderProducts(orderId);
                dataSource = new MvxObservableCollection<IHistoryOrderProductItemVM>(products.Select(p => new HistoryOrderProductItemVM(p)).Cast<IHistoryOrderProductItemVM>());
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

        public async Task<HistoryOrderRepeatResponse> RepeatOrder(string orderId)
		{
            var response = new HistoryOrderRepeatResponse();

			try
			{
                response = await ApiService.RepeatOrder(orderId);
			}
			catch (ConnectionException ex)
			{
                response = null;
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
                response = null;
				OnException(ex);
			}

            return response;
		}

		public void NavigateToProducts(string orderId)
		{
			var ordersHistoryNavigationVmService = Mvx.Resolve<IHistoryOrdersNavigationVmService>();
			var orderBundle = new HistoryOrderBundle(orderId, NavigationType.Push);
            ordersHistoryNavigationVmService.NavigateToOrderProducts(orderBundle);
		}

		public void NavigateToProduct(HistoryOrderProduct product)
		{
            if (!product.IsAvailable)
                return;
            
			var productsNavigationVmService = Mvx.Resolve<IProductsNavigationVmService>();
            var productBundle = new ProductCardBundle(product.GroupId, product.ProductId, NavigationType.Push);
            productsNavigationVmService.NavigateToProduct(productBundle);
		}
    }
}
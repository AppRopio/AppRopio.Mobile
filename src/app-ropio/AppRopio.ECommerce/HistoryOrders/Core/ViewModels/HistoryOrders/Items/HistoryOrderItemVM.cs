using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public class HistoryOrderItemVM : MvxViewModel, IHistoryOrderItemVM
    {
        #region Commands

        private IMvxCommand _goToItemsCommand;

        public IMvxCommand GoToItemsCommand 
        {
			get
			{
                return _goToItemsCommand ?? (_goToItemsCommand = new MvxCommand(OnGoToItems));
			}
        }

		#endregion

		#region Services

		protected IHistoryOrderDetailsVmService VmService { get { return Mvx.IoCProvider.Resolve<IHistoryOrderDetailsVmService>(); } }

        #endregion

        #region Properties

        protected HistoryOrder Order { get; private set; }

        public string OrderId => Order.Id;

        public string OrderNumber => Order.OrderNumber;

        public int ItemsCount => Order.ItemsCount;

        public PaymentStatus PaymentStatus => Order.PaymentStatus;

        public string OrderStatus => Order.OrderStatus;

        public decimal TotalPrice => Order.TotalPrice;

        public string ImageUrl => Order.ImageUrl;

        #endregion

        public HistoryOrderItemVM(HistoryOrder order)
        {
            Order = order;
        }

		protected virtual void OnGoToItems()
		{
            VmService.NavigateToProducts(Order.Id);
		}
    }
}

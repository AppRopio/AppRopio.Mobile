using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public interface IHistoryOrderItemVM : IMvxViewModel
	{
        IMvxCommand GoToItemsCommand { get; }

        string OrderId { get; }

        string OrderNumber { get; }

        int ItemsCount { get; }

        PaymentStatus PaymentStatus { get; }

        string OrderStatus { get; }

        decimal TotalPrice { get; }

        string ImageUrl { get; }
	}
}

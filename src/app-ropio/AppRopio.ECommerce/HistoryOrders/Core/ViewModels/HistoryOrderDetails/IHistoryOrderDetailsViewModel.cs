using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public interface IHistoryOrderDetailsViewModel : IBaseViewModel
    {
        IMvxCommand GoToItemsCommand { get; }

        IMvxCommand RepeatOrderCommand { get; }

        string OrderNumber { get; }

        int ItemsCount { get; }

        List<string> OrderStatus { get; }

        decimal TotalPrice { get; }

        string DeliveryName { get; }

        decimal? DeliveryPrice { get; }

        string DeliveryPointName { get; }

        string DeliveryPointAddress { get; }

        string PaymentName { get; }
    }
}
using System;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items
{
    public interface IHistoryOrderProductItemVM : IMvxViewModel
    {
        HistoryOrderProduct Product { get; }

		string ProductId { get; }

		string GroupId { get; }

		string Title { get; }

		int Amount { get; }

		decimal TotalPrice { get; }

		bool IsAvailable { get; }

		string Badge { get; }

		string ImageUrl { get; }
    }
}

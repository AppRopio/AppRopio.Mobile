using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public interface IHistoryOrderProductsViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        MvxObservableCollection<IHistoryOrderProductItemVM> Items { get; }
    }
}
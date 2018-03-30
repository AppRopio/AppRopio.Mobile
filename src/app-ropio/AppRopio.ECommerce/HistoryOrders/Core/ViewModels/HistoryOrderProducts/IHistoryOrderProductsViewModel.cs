using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
    public interface IHistoryOrderProductsViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        MvxObservableCollection<IHistoryOrderProductItemVM> Items { get; }
    }
}
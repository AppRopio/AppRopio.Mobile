using AppRopio.Base.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
    public interface IHistoryOrdersViewModel : IBaseViewModel, IRefreshViewModel
	{
        IMvxCommand SelectionChangedCommand { get; }

		MvxObservableCollection<IHistoryOrderItemVM> Orders { get; }
	}
}
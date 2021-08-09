using AppRopio.Base.Core.ViewModels;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public interface IHistoryOrdersViewModel : IBaseViewModel, IRefreshViewModel
	{
        IMvxCommand SelectionChangedCommand { get; }

		MvxObservableCollection<IHistoryOrderItemVM> Orders { get; }
	}
}
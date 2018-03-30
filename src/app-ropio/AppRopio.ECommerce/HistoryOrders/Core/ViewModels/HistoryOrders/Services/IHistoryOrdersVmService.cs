using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services
{
    public interface IHistoryOrdersVmService
    {
		Task<MvxObservableCollection<IHistoryOrderItemVM>> LoadHistoryOrders(int count, int offset = 0);

        void HandleOrderSelection(IHistoryOrderItemVM order);
    }
}
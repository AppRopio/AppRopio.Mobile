using System.Threading.Tasks;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services
{
    public interface IHistoryOrderDetailsVmService
    {
        Task<HistoryOrderDetails> LoadOrderDetails(string orderId);

        Task<HistoryOrderRepeatResponse> RepeatOrder(string orderId);

		Task<MvxObservableCollection<IHistoryOrderProductItemVM>> LoadOrderProducts(string orderId);

        void NavigateToProducts(string orderId);

        void NavigateToProduct(HistoryOrderProduct item);
    }
}
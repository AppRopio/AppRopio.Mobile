using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.HistoryOrders.Responses;

namespace AppRopio.ECommerce.HistoryOrders.API.Services
{
    public interface IHistoryOrdersService
    {
        Task<List<HistoryOrder>> GetOrders(int count, int offset = 0);

        Task<HistoryOrderDetails> GetOrderDetails(string orderId);

        Task<List<HistoryOrderProduct>> GetOrderProducts(string orderId);

        Task<HistoryOrderRepeatResponse> RepeatOrder(string orderId);
    }
}
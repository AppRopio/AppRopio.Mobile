using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.HistoryOrders.Responses;

namespace AppRopio.ECommerce.HistoryOrders.API.Services.Implementation
{
    public class HistoryOrdersService : BaseService, IHistoryOrdersService
    {
        protected string HISTORY_URL = "history?count={0}&offset={1}";
        protected string HISTORY_DETAILS_URL = "history/orderDetails?id={0}";
        protected string HISTORY_ORDER_PRODUCTS_URL = "history/orderProducts?id={0}";
        protected string HISTORY_REPEAT_URL = "history/repeat?orderId={0}";

        public async Task<List<HistoryOrder>> GetOrders(int count, int offset = 0)
        {
            var url = string.Format(HISTORY_URL, count, offset);
            return await Get<List<HistoryOrder>>(url);
        }

        public async Task<HistoryOrderDetails> GetOrderDetails(string orderId)
		{
            var url = string.Format(HISTORY_DETAILS_URL, orderId);
            return await Get<HistoryOrderDetails>(url);
		}

        public async Task<List<HistoryOrderProduct>> GetOrderProducts(string orderId)
		{
			var url = string.Format(HISTORY_ORDER_PRODUCTS_URL, orderId);
            return await Get<List<HistoryOrderProduct>>(url);
		}

        public async Task<HistoryOrderRepeatResponse> RepeatOrder(string orderId)
		{
            var url = string.Format(HISTORY_REPEAT_URL, orderId);
            return await Get<HistoryOrderRepeatResponse>(url);
		}
    }
}
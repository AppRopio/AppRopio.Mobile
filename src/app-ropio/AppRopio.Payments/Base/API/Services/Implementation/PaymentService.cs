using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.API.Services.Implementation
{
    public class PaymentService : BaseService, IPaymentService
    {
        protected string ORDER_INFO = "payments/orderInfo";

		public async Task<PaymentOrderInfo> OrderInfo(string orderId)
		{
            return await Get<PaymentOrderInfo>($"{ORDER_INFO}?id={orderId}");
		}
	}
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.API.Services.Fake
{
    public class PaymentFakeService : IPaymentService
    {
        private PaymentOrderInfo _orderInfo = new PaymentOrderInfo()
        {
            Amount = 1,
            Currency = "RUB",
            CustomerPhone = "+79211231231",
            CustomerEmail = "a@a.com",
            Items = new List<PaymentOrderItem> 
            {
				new PaymentOrderItem
				{
					Title = "Угловой диван Камелот",
                    Amount = 75,
                    Quantity = 1,
				},
                new PaymentOrderItem
				{
                    Title = "Доставка",
                    Amount = 25,
                    Quantity = 1
				},
            }
        };

        public async Task<PaymentOrderInfo> OrderInfo(string orderId)
        {
            await Task.Delay(500);

            return _orderInfo;
        }
    }
}
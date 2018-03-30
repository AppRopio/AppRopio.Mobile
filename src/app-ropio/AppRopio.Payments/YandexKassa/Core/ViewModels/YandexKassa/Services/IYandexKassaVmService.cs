using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.YandexKassa.Core.Services;

namespace AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa.Services
{
    public interface IYandexKassaVmService
    {
        Task<PaymentOrderInfo> GetPaymentInfo(string orderId);

        Task<bool> OrderPaid(string orderId);

        Dictionary<string, string> GetPaymentParams(decimal amount, string currency, string orderId, IList<PaymentOrderItem> items, string customerPhone);
		
        Task<bool> PayWithApplePay(string paymentData, decimal amount, string currency, string orderId);
    }
}
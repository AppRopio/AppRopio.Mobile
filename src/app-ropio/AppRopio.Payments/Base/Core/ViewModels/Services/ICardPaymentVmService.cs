using System;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Core.Models;

namespace AppRopio.Payments.Core.ViewModels.Services
{
    public interface ICardPaymentVmService
    {
        Task<PaymentOrderInfo> GetPaymentInfo(string orderId);

        Task<PaymentResult> PayWithCard(string cardNumber, string expirateDate, string cvv, string cardHolderName, decimal amount, string currency, Action threeDSCallback, string orderId);
    }
}
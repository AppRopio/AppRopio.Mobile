using System;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay.Services
{
    public interface IBest2PayVmService
    {
        Task<PaymentOrderInfo> GetPaymentInfo(string orderId);

        Task<int> RegisterOrder(decimal amount, int currency, string email, string phone, string orderId);

        string GetPurchaseUrl(int orderId, string email = null);

        Task<bool> ProcessPayment(string url);
    }
}
using System;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentOrderInfo> OrderInfo(string orderId);
    }
}
using System;
using System.Threading.Tasks;
using AppRopio.Payments.Core.Models;
using AppRopio.Payments.Core.ViewModels.Services;

namespace AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services
{
    public interface ICloudPaymentsVmService : ICardPaymentVmService
    {
        Task<PaymentResult> PayWithApplePay(string token, decimal amount, string currency, string orderId);
    }
}
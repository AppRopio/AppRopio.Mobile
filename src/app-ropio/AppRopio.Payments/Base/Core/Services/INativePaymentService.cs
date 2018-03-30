using System;
using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.Core.Services
{
    public interface INativePaymentService
    {
        Task<string> Pay(PaymentOrderInfo info);

        void CompleteSuccess();

        void CompleteFail();
    }
}
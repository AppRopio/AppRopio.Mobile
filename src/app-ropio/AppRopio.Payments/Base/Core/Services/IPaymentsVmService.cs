using System;
using System.Threading.Tasks;

namespace AppRopio.Payments.Core.Services
{
    public interface IPaymentsVmService
    {
        Task OrderPaid(string orderId);

        Task OrderPaidWithTransactionId(string orderId, string transactionId);
    }
}
using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.Payments.Core.Messages;

namespace AppRopio.Payments.Core.Services.Implementation
{
    public class PaymentsVmService : BaseVmService, IPaymentsVmService
    {
        public virtual Task OrderPaid(string orderId)
        {
            return Task.Run(() => Mvx.Resolve<IMvxMessenger>().Publish(new OrderPaidMessage(this, orderId)));
        }

        public virtual Task OrderPaidWithTransactionId(string orderId, string transactionId)
        {
            return Task.Run(() => Mvx.Resolve<IMvxMessenger>().Publish(new OrderPaidMessage(this, orderId, transactionId)));
        }
    }
}

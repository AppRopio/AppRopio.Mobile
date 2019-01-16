using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Payments.Core.Messages
{
    public class OrderPaidMessage : MvxMessage
    {
        public string OrderId { get; }

        public string TransactionId { get; }

        public OrderPaidMessage(object sender, string orderId)
            : base(sender)
        {
            OrderId = orderId;
        }

        public OrderPaidMessage(object sender, string orderId, string transactionId)
            : base(sender)
        {
            OrderId = orderId;
            TransactionId = transactionId;
        }
    }
}
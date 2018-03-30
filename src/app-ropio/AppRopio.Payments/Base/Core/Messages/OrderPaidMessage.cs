using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Payments.Core.Messages
{
    public class OrderPaidMessage : MvxMessage
    {
        public string OrderId { get; }

        public OrderPaidMessage(object sender, string orderId)
            : base (sender)
        {
            OrderId = orderId;
        }
    }
}

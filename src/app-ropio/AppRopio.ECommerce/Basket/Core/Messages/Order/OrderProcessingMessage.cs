using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Order
{
    public class OrderProcessingMessage : MvxMessage
    {
        public bool OrderingInProcess { get; }

        public OrderProcessingMessage(object sender, bool orderingInProcess)
            : base (sender)
        {
            OrderingInProcess = orderingInProcess;
        }
    }
}

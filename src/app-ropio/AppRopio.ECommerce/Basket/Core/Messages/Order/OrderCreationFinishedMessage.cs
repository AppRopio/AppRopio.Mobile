using System;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Order
{
    public class OrderCreationFinishedMessage : MvxMessage
    {
        public bool IsSuccessful { get; }

        public OrderCreationFinishedMessage(object sender, bool isSuccessful)
            : base (sender)
        {
            IsSuccessful = isSuccessful;
        }
    }
}

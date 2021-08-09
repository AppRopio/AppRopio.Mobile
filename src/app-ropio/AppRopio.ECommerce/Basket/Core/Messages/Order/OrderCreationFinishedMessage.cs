using System;
using MvvmCross.Plugin.Messenger;

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

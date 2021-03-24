using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Messages.Basket
{
    public class ItemDeletedMessage : MvxMessage
    {
        public string Id { get; set; }

        public ItemDeletedMessage(object sender)
            : base (sender)
        {
        }
    }
}

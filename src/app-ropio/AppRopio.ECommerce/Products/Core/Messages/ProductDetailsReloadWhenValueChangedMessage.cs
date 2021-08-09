using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductDetailsReloadWhenValueChangedMessage : MvxMessage
    {
        public ProductDetailsReloadWhenValueChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}

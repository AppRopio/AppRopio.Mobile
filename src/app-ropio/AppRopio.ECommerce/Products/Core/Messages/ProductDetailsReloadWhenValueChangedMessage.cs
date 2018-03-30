using System;
using MvvmCross.Plugins.Messenger;

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

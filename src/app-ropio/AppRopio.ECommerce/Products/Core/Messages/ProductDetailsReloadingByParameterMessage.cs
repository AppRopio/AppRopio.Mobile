using System;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductDetailsReloadingByParameterMessage : MvxMessage
    {
        public bool Reloading { get; }

        public ProductDetailsReloadingByParameterMessage(object sender, bool reloading)
            : base(sender)
        {
            Reloading = reloading;
        }
    }
}

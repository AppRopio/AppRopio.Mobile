using System;
using MvvmCross.Plugins.Messenger;
namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductMarkedQuantityChangedMessage : MvxMessage
    {
        public ProductMarkedQuantityChangedMessage(object sender)
            : base (sender)
        {
        }
    }
}

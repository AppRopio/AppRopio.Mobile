using System;
using MvvmCross.Plugin.Messenger;
namespace AppRopio.ECommerce.Basket.Core.Messages
{
    public class ProductAddToBasketMessage : MvxMessage
    {
        public string ProductId { get; }

        public ProductAddToBasketMessage(object sender, string productId)
            : base (sender)
        {
            ProductId = productId;
        }

        public ProductAddToBasketMessage(object sender)
            : base(sender)
        {
            
        }
    }
}

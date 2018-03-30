using AppRopio.Models.Products.Responses;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductMarkChangedMessage : MvxMessage
    {
        public Product Model { get; set; }

        public bool Marked { get; set; }

        public ProductMarkChangedMessage(object sender, Product model, bool isMarked)
            : base(sender)
        {
            Model = model;
            Marked = isMarked;
        }
    }
}

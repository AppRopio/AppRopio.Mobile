using System.Collections.Generic;
using AppRopio.Models.Products.Responses;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.Messages
{
    public class ProductDetailsSelectionChangedMessage : MvxMessage
    {
        public string ParameterId { get; set; }

        public List<ApplyedProductParameterValue> ApplyedParameterValues { get; set; }

        public ProductDetailsSelectionChangedMessage(object sender)
            : base(sender)
        {
        }
    }
}

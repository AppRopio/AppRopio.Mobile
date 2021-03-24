using System;
using MvvmCross.Plugin.Messenger;
namespace AppRopio.ECommerce.Basket.Core.Messages.Order
{
    public class DeliveryConfirmedMessage : MvxMessage
    {
        public string DeliveryId { get; set; }

        public decimal? DeliveryPrice { get; set; }

        public DeliveryConfirmedMessage(object sender)
            : base (sender)
        {
        }
    }
}

using System;
using MvvmCross.Plugin.Messenger;
using AppRopio.Models.Basket.Responses.Order;

namespace AppRopio.ECommerce.Basket.Core.Messages
{
    public class PaymentSelectedMessage : MvxMessage
    {
        public Payment Payment { get; }

        public PaymentSelectedMessage(object sender, Payment payment)
            : base (sender)
        {
            Payment = payment;
        }
    }
}

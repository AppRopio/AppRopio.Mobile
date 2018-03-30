using System;
using AppRopio.Payments.YandexKassa.API.Models;

namespace AppRopio.Payments.YandexKassa.API.Requests
{
    public abstract class PaymentPayloadBase
    {
        public Recipient Recipient { get; set; }

        public Order Order { get; set; }

        public PaymentPayloadBase()
        {
            Recipient = new Recipient();

            Order = new Order();
        }
    }
}
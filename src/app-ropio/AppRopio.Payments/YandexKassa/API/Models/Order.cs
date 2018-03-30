using System;
namespace AppRopio.Payments.YandexKassa.API.Models
{
    public class Order
    {
        public string ClientOrderId { get; set; }

        public OrderValue Value { get; set; }

        public Order()
        {
            Value = new OrderValue();
        }
    }
}
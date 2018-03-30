using System;
namespace AppRopio.Payments.YandexKassa.API.Models
{
    public class OrderValue
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
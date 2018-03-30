using System;
namespace AppRopio.Payments.YandexKassa.Core.Models
{
    public class YandexKassaConfig
    {
        public string ShopId { get; set; }

        public string Scid { get; set; }

        public string SuccessUrl { get; set; }

        public string FailUrl { get; set; }

        public string ShopUrl { get; set; }
    }
}

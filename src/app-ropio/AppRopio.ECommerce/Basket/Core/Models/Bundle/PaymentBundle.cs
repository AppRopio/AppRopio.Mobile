using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
    public class PaymentBundle : BaseBundle
    {
        public string DeliveryId { get; set; }

        public PaymentBundle()
        {
        }

        public PaymentBundle(string deliveryId, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string> { { nameof(DeliveryId), deliveryId } })
        {
            DeliveryId = deliveryId;
        }
    }
}

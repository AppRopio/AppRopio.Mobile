using System;
using System.Collections.Generic;
using PassKit;

namespace AppRopio.Payments.ApplePay.Models
{
    public class ApplePayConfig
    {
        public string MerchantId { get; set; }

        public string CountryCode { get; set; }

        public List<string> SupportedNetworks { get; set; }
    }
}
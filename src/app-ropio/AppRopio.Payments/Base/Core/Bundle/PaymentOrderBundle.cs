using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Models.Payments.Responses;

namespace AppRopio.Payments.Core.Bundle
{
    public class PaymentOrderBundle : BaseBundle
    {
		public string OrderId { get; set; }

        public PaymentType PaymentType { get; set; }

		public PaymentOrderBundle()
		{
		}

        public PaymentOrderBundle(string orderId, PaymentType paymentType, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string> {
            { nameof(OrderId), orderId },
            { nameof(PaymentType), paymentType.ToString() }
        })
        {
			OrderId = orderId;
            PaymentType = paymentType;
		}
    }
}

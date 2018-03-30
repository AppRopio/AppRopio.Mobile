using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
	public class ThanksBundle : BaseBundle
	{
		public string OrderId { get; set; }

		public ThanksBundle()
		{
		}

		public ThanksBundle(string orderId, NavigationType navigationType)
			: base(navigationType, new Dictionary<string, string> { { nameof(OrderId), orderId } })
		{
            OrderId = orderId;
		}
	}
}

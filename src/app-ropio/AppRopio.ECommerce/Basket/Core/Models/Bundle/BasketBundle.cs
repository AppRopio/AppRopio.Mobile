using System.Collections.Generic;
using System.Globalization;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
    public class BasketBundle : BaseBundle
    {
        public decimal BasketAmount { get; set; }

        public BasketBundle()
        {
        }

        public BasketBundle(NavigationType navigationType, decimal basketAmount)
            : base(navigationType, new Dictionary<string, string> { { nameof(BasketAmount), basketAmount.ToString(NumberFormatInfo.InvariantInfo) } })
        {
        }
    }
}

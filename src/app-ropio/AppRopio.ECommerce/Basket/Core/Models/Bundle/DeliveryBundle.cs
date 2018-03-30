using System;
using System.Collections.Generic;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using System.Globalization;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
    public class DeliveryBundle : BaseBundle
    {
        public string Id { get; set; }

        public DeliveryType Type { get; set; }

        public decimal BasketAmount { get; set; }

        public DeliveryBundle()
        {
        }

        public DeliveryBundle(string id, DeliveryType type, decimal basketAmount, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string> 
        {
            { nameof(Id), id }, 
            { nameof(Type), ((int)type).ToString() }, 
            { nameof(BasketAmount), basketAmount.ToString(NumberFormatInfo.InvariantInfo) } 
        })
        {
            Id = id;
            Type = type;
            BasketAmount = basketAmount;
        }
    }
}

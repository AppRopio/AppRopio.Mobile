using System.Collections.Generic;
using System.Globalization;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle
{
    public class HistoryOrderBundle : BaseBundle
    {
        public string OrderId { get; private set; }

        public string OrderNumber { get; private set; }

        public decimal TotalPrice { get; private set; }

        public int ItemsCount { get; private set; }

        public HistoryOrderBundle()
        {

        }

        public HistoryOrderBundle(string orderId, string orderNumber, decimal totalPrice, int itemsCount, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
        {
            [nameof(OrderId)] = orderId,
            [nameof(OrderNumber)] = orderNumber,
            [nameof(TotalPrice)] = totalPrice.ToString(NumberFormatInfo.InvariantInfo),
            [nameof(ItemsCount)] = itemsCount.ToString(NumberFormatInfo.InvariantInfo)
        })
        {

        }

        public HistoryOrderBundle(string orderId, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                [nameof(OrderId)] = orderId,
            })
        {

        }
    }
}
using System.Collections.Generic;

namespace AppRopio.Base.Core.Messages.Analytics
{
    public class OrderAnalyticsMessage : BaseAnalyticsMessage
    {
        public decimal FullPrice { get; private set; }

        public float Quantity { get; private set; }

        public string OrderId { get; private set; }

        public string Currency { get; private set; }

        public OrderAnalyticsMessage(object sender, decimal fullPrice, float quantity, string orderId, string currency, Dictionary<string, string> data)
            : base(sender, data)
        {
            Currency = currency;
            OrderId = orderId;
            Quantity = quantity;
            FullPrice = fullPrice;
        }
    }
}

using System;

namespace AppRopio.Payments.CloudPayments.API.Requests
{
    public class ChargeRequest
    {
        public string CardCryptogramPacket { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Name { get; set; }

        public int InvoiceId { get; set; }
    }
}
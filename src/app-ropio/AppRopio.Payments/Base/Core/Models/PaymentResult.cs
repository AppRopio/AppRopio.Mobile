using System;

namespace AppRopio.Payments.Core.Models
{
    public class PaymentResult
    {
        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public string TransactionId { get; set; }
    }
}

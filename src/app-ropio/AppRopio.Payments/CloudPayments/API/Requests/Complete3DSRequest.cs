using System;
namespace AppRopio.Payments.CloudPayments.API.Requests
{
    public class Complete3DSRequest
    {
        public string PaRes { get; set; }

        public string TransactionId { get; set; }
    }
}
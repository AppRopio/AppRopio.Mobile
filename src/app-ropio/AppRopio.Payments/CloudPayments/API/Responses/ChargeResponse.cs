using System;
namespace AppRopio.Payments.CloudPayments.API.Responses
{
    public class ChargeResponse
    {
        public string TransactionId { get; set; }

        public string PaReq { get; set; }

        public string AcsUrl { get; set; }

        public bool IFrameIsAllowed { get; set; }

        public string CardHolderMessage { get; set; }
    }
}
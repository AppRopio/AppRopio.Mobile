using System;
namespace AppRopio.Payments.YandexKassa.API.Requests
{
    public class DsrpWalletPayload : PaymentPayloadBase
    {
        public string Source { get; set; }

        public string WalletType { get; set; }

        public string PaymentData { get; set; }
    }
}
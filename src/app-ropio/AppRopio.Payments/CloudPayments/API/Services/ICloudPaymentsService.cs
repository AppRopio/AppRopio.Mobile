using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Payments.CloudPayments.API.Responses;

namespace AppRopio.Payments.CloudPayments.API.Services
{
    public interface ICloudPaymentsService
    {
        Task<Response<ChargeResponse>> Charge(string cardCryptogram, decimal amount, string currency, string name, string publicId, string apiSecret, string orderId);

        Task<Response<ChargeResponse>> Auth(string cardCryptogram, decimal amount, string currency, string name, string publicId, string apiSecret, string orderId);

        Task<Response<Complete3DSResponse>> Completed3DSPayment(string paRes, string transactionId, string publicId, string apiSecret);

        Dictionary<string, string> Get3DSPaymentParams(ChargeResponse chargeResponse, string redirectUrl);

        string CreateCryptogramPacket(string cardNumber, string expDateString, string cvv, string publicKey, string publicId);
    }
}
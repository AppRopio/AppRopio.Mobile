using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Models;
using AppRopio.Payments.CloudPayments.API.Requests;
using AppRopio.Payments.CloudPayments.API.Responses;
using Newtonsoft.Json;
using PCLCrypto;

namespace AppRopio.Payments.CloudPayments.API.Services.Implementation
{
    public class CloudPaymentsService : ICloudPaymentsService
    {
        protected const string BASE_URL = "https://api.cloudpayments.ru/payments/";
        protected const string CHARGE = "cards/charge";
        protected const string AUTH = "cards/auth";
        protected const string POST_3DS = "mobile/cards/post3ds";

        public async Task<Response<ChargeResponse>> Charge(string cardCryptogram, decimal amount, string currency, string name, string publicId, string apiSecret, string orderId)
        {
            var httpClient = new HttpClient();

            var request = new ChargeRequest()
            {
                CardCryptogramPacket = cardCryptogram,
                Amount = amount,
                Currency = currency,
                Name = name,
                InvoiceId = Convert.ToInt32(orderId)
            };

            return await Post<Response<ChargeResponse>>(CHARGE, ToStringContent(request), publicId, apiSecret);
        }

        public async Task<Response<ChargeResponse>> Auth(string cardCryptogram, decimal amount, string currency, string name, string publicId, string apiSecret, string orderId)
        {
            var httpClient = new HttpClient();

            var request = new ChargeRequest()
            {
                CardCryptogramPacket = cardCryptogram,
                Amount = amount,
                Currency = currency,
                Name = name,
                InvoiceId = Convert.ToInt32(orderId)
            };

            return await Post<Response<ChargeResponse>>(AUTH, ToStringContent(request), publicId, apiSecret);
        }

        public async Task<Response<Complete3DSResponse>> Completed3DSPayment(string paRes, string transactionId, string publicId, string apiSecret)
        {
            var httpClient = new HttpClient();

            var request = new Complete3DSRequest()
            {
                PaRes = paRes,
                TransactionId = transactionId
            };

            return await Post<Response<Complete3DSResponse>>(POST_3DS, ToStringContent(request), publicId, apiSecret);
        }

        public Dictionary<string, string> Get3DSPaymentParams(ChargeResponse chargeResponse, string redirectUrl)
        {
            return new Dictionary<string, string>
            {
                ["MD"] = chargeResponse.TransactionId,
                ["TermUrl"] = redirectUrl,
                ["PaReq"] = chargeResponse.PaReq
            };
        }

        public string CreateCryptogramPacket(string cardNumber, string expDateString, string cvv, string publicKey, string publicId)
        {
            string result = "";
            string[] expDate = expDateString.Split(new[] { '/' });
            int expDateMonth = Convert.ToInt32(expDate[0]);
            int expDateYear = Convert.ToInt32(expDate[1]);
            string ExpDate = NumberToEvenLengthString(expDateYear) + NumberToEvenLengthString(expDateMonth);
            result = cardNumber + "@" + ExpDate + "@" + cvv + "@" + publicId;

            byte[] keyBuffer = WinRTCrypto.CryptographicBuffer.DecodeFromBase64String(publicKey);
            var asym = WinRTCrypto.AsymmetricKeyAlgorithmProvider.OpenAlgorithm(AsymmetricAlgorithm.RsaPkcs1);
            ICryptographicKey key = asym.ImportPublicKey(keyBuffer, CryptographicPublicKeyBlobType.X509SubjectPublicKeyInfo);
            byte[] plainBuffer = WinRTCrypto.CryptographicBuffer.ConvertStringToBinary(result, Encoding.UTF8);
            byte[] encryptedBuffer = WinRTCrypto.CryptographicEngine.Encrypt(key, plainBuffer, null);

            byte[] encryptedBytes;
            WinRTCrypto.CryptographicBuffer.CopyToByteArray(encryptedBuffer, out encryptedBytes);
            result = Convert.ToBase64String(encryptedBytes);
            result = "01" + cardNumber.Substring(0, 6) + cardNumber.Substring(cardNumber.Length - 4) + NumberToEvenLengthString(expDateYear) + NumberToEvenLengthString(expDateMonth) + "02" + result;
            return result;
        }

        private string NumberToEvenLengthString(int num)
        {
            string result = "";
            if (num < 10)
                result = "0" + num.ToString();
            else
                result = num.ToString();
            return result;
        }

        protected StringContent ToStringContent(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data) ?? string.Empty, System.Text.Encoding.UTF8, "application/json");
        }


        internal static async Task<T> Post<T>(string url, HttpContent postContent, string publicId, string apiSecret)
        {
            var uri = new Uri($"{BASE_URL}{url}");

            var httpClient = new HttpClient();
            var byteArray = Encoding.UTF8.GetBytes($"{publicId}:{apiSecret}");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, postContent);
            var response = await responseMessage.Content.ReadAsStringAsync();

            Task.Run(() => WriteInOutput(uri.ToString(), response, postContent));

            if (!string.IsNullOrEmpty(response))
            {
                return JsonConvert.DeserializeObject<T>(response);
            }

            return default(T);
        }


        protected static async Task WriteInOutput(string url, string result, HttpContent postData = null)
        {
            try
            {
                var rawContent = result;//.ResponseCode == System.Net.HttpStatusCode.NoContent ? string.Empty : (result.Exception == null ? (await ReadContentAsString(result)) : result.Exception.Message);

                var responseMessage = DecodeEncodedNonAsciiCharacters(rawContent);

                string formatedOutput = string.Empty;
                formatedOutput += $"{Environment.NewLine}{Environment.NewLine}";
                formatedOutput += $"# REQUEST {Environment.NewLine}";

                //var requestHeaders = ConnectionService.Headers;
                //var responseHeaders = result.Headers;

                //formatedOutput += $"## Request headers {Environment.NewLine}```{Environment.NewLine}{JsonConvert.SerializeObject(requestHeaders)}{Environment.NewLine}```{Environment.NewLine}";

                formatedOutput += $"## Url{Environment.NewLine}`{url}`{Environment.NewLine}";

                //formatedOutput += $"## Status code{Environment.NewLine}`{(int)result.ResponseCode} {result.ResponseCode}`{Environment.NewLine}";

                if (postData != null)
                {
                    var json = await postData.ReadAsStringAsync();
                    formatedOutput += $"## Request message{Environment.NewLine}`{json}`{Environment.NewLine}";
                }
                //formatedOutput += $"## Time{Environment.NewLine}`{stopwatch.ElapsedMilliseconds} ms`{Environment.NewLine}";
                formatedOutput += $"# RESPONSE{Environment.NewLine}";
                formatedOutput += $"## Response length{Environment.NewLine}`{(responseMessage == null ? 0 : responseMessage.Length)}`{Environment.NewLine}";
                //formatedOutput += $"## Response headers{Environment.NewLine}```{Environment.NewLine}{JsonConvert.SerializeObject(responseHeaders)}{Environment.NewLine}```{Environment.NewLine}";
                formatedOutput += $"## Response message{Environment.NewLine}```{Environment.NewLine}{responseMessage}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}";

                //Trace.Trace(MvxTraceLevel.Diagnostic, $"{GetType().FullName}", formatedOutput);
            }
            catch (Exception ex)
            {
                //Trace.Trace(MvxTraceLevel.Error, "WriteOut", ex.StackTrace);
            }

        }

        private async Task<string> ReadContentAsString(RequestResult result)
        {
            var content = string.Empty;

            try
            {
                content = await result.ResponseContent.ReadAsStringAsync();
            }
            catch
            { }

            return content;
        }

        private static string DecodeEncodedNonAsciiCharacters(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m =>
                {
                    if (int.TryParse(m.Groups["Value"].Value, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out int result))
                        return ((char)result).ToString();
                    return string.Empty;
                });
        }
    }
}
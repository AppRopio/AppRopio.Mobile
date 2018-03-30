using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppRopio.Payments.YandexKassa.API.Requests;
using Newtonsoft.Json;

namespace AppRopio.Payments.YandexKassa.API.Services.Implementation
{
    public class YandexKassaService : IYandexKassaService
    {
		protected const string BASE_URL = "https://payment.yandex.net/api/v2/";
        protected const string DSRPWALLET = "payments/dsrpWallet";

		public async Task DsrpWallet(string shopId, string orderId, decimal amount, string currency, string paymentData)
		{
			var httpClient = new HttpClient();

            var payload = new DsrpWalletPayload();
            payload.Recipient.ShopId = shopId;
            payload.Order.ClientOrderId = orderId;
            payload.Order.Value.Amount = amount;
            payload.Order.Value.Currency = currency;
            payload.Source = "BankCard";
            payload.WalletType = "ApplePay";
            payload.PaymentData = Convert.ToBase64String(Encoding.UTF8.GetBytes(paymentData));

            var headers = new Dictionary<string, object>
            {
                ["iss"] = $"shopId:{shopId}"
            };
            var request = Encode(headers, payload);

            //TODO
            await Post<string>(DSRPWALLET, ToFormContent(request));
		}

        protected HttpContent ToFormContent(string data)
		{
            return new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["request"] = data
            });
		}

        protected string Encode(Dictionary<string, object> extraHeaders, object payload)
        {
            var segments = new List<string>();

            var headers = new Dictionary<string, object>(extraHeaders)
            {
                ["alg"] = "ES256",
                ["iat"] = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds
            };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(headers));
			byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

			segments.Add(Base64UrlEncode(headerBytes));
			segments.Add(Base64UrlEncode(payloadBytes));

			var stringToSign = string.Join(".", segments.ToArray());

			var bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = SHA256(bytesToSign);
			segments.Add(Base64UrlEncode(signature));

			return string.Join(".", segments.ToArray());
        }

        internal static byte[] SHA256(byte[] input)
        {
            //TODO
            byte[] buffer = PCLCrypto.WinRTCrypto.CryptographicBuffer.CreateFromByteArray(input);
            var hasher = PCLCrypto.WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(PCLCrypto.HashAlgorithm.Sha256);
			return hasher.HashData(input);
        }

		// from JWT spec
        internal static string Base64UrlEncode(byte[] input)
		{
			var output = Convert.ToBase64String(input);
			output = output.Split('=')[0]; // Remove any trailing '='s
			output = output.Replace('+', '-'); // 62nd char of encoding
			output = output.Replace('/', '_'); // 63rd char of encoding
			return output;
		}

		internal static async Task<T> Post<T>(string url, HttpContent postContent)
		{
			var uri = new Uri($"{BASE_URL}{url}");

			var httpClient = new HttpClient();

			HttpResponseMessage responseMessage = await httpClient.PostAsync(uri, postContent);
			var response = await responseMessage.Content.ReadAsStringAsync();
			if (!string.IsNullOrEmpty(response))
			{
				return JsonConvert.DeserializeObject<T>(response);
			}

			return default(T);
		}
    }
}
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;

namespace AppRopio.Base.API.Services.Implementations
{
    public class ErrorService : BaseService, IErrorService
    {
        private const string URL = "https://api.notissimus.com/appstat/logerror";

        public void Send(string message, string stackTrace, string packageName, string appVersion, string deviceName, byte[] data = null)
        {
            var errorInfo = new
            {
                Message = message,
                StackTrace = stackTrace,
                DeviceName = deviceName,
                //FromScreen = _firstScreen,
                //ToScreen = _secondScreen,
#if DEBUG
                IsDebug = true,
#endif
                AppName = packageName,
                AppVersion = appVersion,
                Data = data
            };

            using (HttpClient httpClient = new HttpClient())
            {
                using (HttpRequestMessage httpMessage = new HttpRequestMessage())
                {
                    httpMessage.RequestUri = new Uri(URL);

                    //httpMessage.Headers.Add(HeaderInfo.HeaderName, WebServiceStatistics.HeaderInfo);

                    httpMessage.Method = HttpMethod.Post;

                    var json = JsonConvert.SerializeObject(errorInfo);

                    httpMessage.Content = new StringContent(json ?? string.Empty, System.Text.Encoding.UTF8, "application/json"); ;
                    httpMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json; charset=utf-8");

                    var token = new CancellationTokenSource().Token;
                    var result = httpClient.SendAsync(httpMessage, token).Result;
#if DEBUG
                    if (result.IsSuccessStatusCode)
                        System.Diagnostics.Debug.WriteLine("\nERROR HAS BEEN SENT\n");
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"\nERROR HAS NOT BEEN SENT\nREQUEST:{json}\nRESPONSE:{result.Content.ReadAsStringAsync().Result}\n");
                    }
#endif
                }
            }
        }
    }
}
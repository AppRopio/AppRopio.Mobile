using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Models;

namespace AppRopio.Base.API.Services
{
    public class ConnectionService : IConnectionService
    {
        private bool _isDisposed;

        private readonly object _requestsLocker = new object();

        private Lazy<HttpClient> _explicit;

        public virtual Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        public virtual string AcceptHeader { get; set; } = "application/json";

        public HttpMessageHandler HttpHandler { get; private set; }

        public Func<Task<bool>> IsConnectionAvailable { get; set; }

        private Func<HttpMessageHandler> _createHandlerFunc;
        public Func<HttpMessageHandler> CreateHandlerFunc
        {
            get
            {
                return _createHandlerFunc;
            }
            set
            {
                _createHandlerFunc = value;
            }
        }

        public virtual int RequestTimeoutInSeconds { get; set; } = 30;

        private string _errorWhenConnectionFailed = "Отсутствует интернет соединение!";
        public virtual string ErrorWhenConnectionFailed
        {
            get { return _errorWhenConnectionFailed; }
            set
            {
                _errorWhenConnectionFailed = value;
            }
        }

        private string _errorWhenTaskCanceled = "Отсутствует интернет соединение!";
        public virtual string ErrorWhenTaskCanceled
        {
            get { return _errorWhenTaskCanceled; }
            set
            {
                _errorWhenTaskCanceled = value;
            }
        }

        private Uri _baseUrl;
        public Uri BaseUrl
        {
            get
            {
                return _baseUrl;
            }
            set
            {
                _baseUrl = value;
            }
        }

        public ConnectionService()
        {
            HttpClient CreateClient(HttpMessageHandler messageHandler) => new HttpClient(messageHandler)
            {
                Timeout = TimeSpan.FromSeconds(RequestTimeoutInSeconds),
                BaseAddress = BaseUrl
            };

            _explicit = new Lazy<HttpClient>(() => CreateClient((HttpHandler = CreateHandlerFunc?.Invoke() ?? CreateHandler())));
        }

        private HttpClient GetHttpClient()
        {
            return _explicit.Value;
        }

        private void SetHttpRequestHeaders(HttpRequestMessage message)
        {
            message.Headers?.Clear();

            if (Headers != null && Headers.Any())
            {
                foreach (var header in Headers)
                    message.Headers.Add(header.Key, header.Value);
            }

            if (!message.Headers.Accept.Any(x => x.MediaType == AcceptHeader))
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(AcceptHeader));

            if (!message.Headers.AcceptEncoding.Any(x => x.Value == "gzip"))
                message.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
        }

        private async Task<RequestResult> SendRequest(HttpMethod method, string url, CancellationToken? token, HttpContent httpContent = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            var cancellationToken = token ?? new CancellationToken(false);

            if (!(await IsConnectionAvailable()))
            {
                return new RequestResult
                {
                    Succeeded = false,
                    RequestCanceled = true,
                    RequestCancelReason = RequestCancelReason.ConnectionFailed,
                    Exception = new Exception(ErrorWhenConnectionFailed)
                };
            }

            RequestResult requestResult = null;

            try
            {
                var httpClient = GetHttpClient();

                var httpRequestMessage = new HttpRequestMessage(method, $"{BaseUrl?.OriginalString ?? string.Empty}{url}") { Content = httpContent };

                SetHttpRequestHeaders(httpRequestMessage);

                requestTuneAction?.Invoke(httpRequestMessage);

                System.Diagnostics.Debug.WriteLine($"Diagnostic: ConnectionService: {method} request sending {httpRequestMessage.RequestUri}");

                var result = await httpClient.SendAsync(httpRequestMessage, cancellationToken).ConfigureAwait(false);

                System.Diagnostics.Debug.WriteLine($"Diagnostic: ConnectionService: {method} request result recieved. Status code: {result.StatusCode}");

                requestResult = new RequestResult
                {
                    Succeeded = result.IsSuccessStatusCode,
                    Headers = result.Headers.ToDictionary(x => x.Key, x => x.Value),

                    ResponseCode = result.StatusCode,
                    ResponseHeaders = result.Content.Headers.ToDictionary(x => x.Key, x => x.Value),

                    ResponseContent = result.Content
                };
            }
            catch (TaskCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: ConnectionService: {ex.GetType()}: {ex.Message}\n {ex.StackTrace}");

                requestResult = new RequestResult
                {
                    Succeeded = false,
                    Exception = new Exception(ErrorWhenTaskCanceled, ex),
                    RequestCanceled = true,
                    RequestCancelReason = cancellationToken.IsCancellationRequested ? RequestCancelReason.Manual : RequestCancelReason.Timeout
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: ConnectionService: {ex.GetType()}: {ex.Message}\n {ex.StackTrace}");

                requestResult = new RequestResult
                {
                    Succeeded = false,
                    Exception = ex
                };
            }

            return requestResult;
        }

        private async Task<string> ReadContentAsStringAsync(HttpContent content)
        {
            string result = null;

            try
            {
                result = content == null ? null : await content.ReadAsStringAsync();
            }
            catch (Exception)
            { }

            return result;
        }

        protected virtual HttpClientHandler CreateHandler()
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            return handler;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _explicit.Value?.Dispose();
            }

            _explicit = null;

            _isDisposed = true;
        }

        public virtual async Task<RequestResult> GET(string url, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            return await SendRequest(HttpMethod.Get, url, token, null, requestTuneAction).ConfigureAwait(false);
        }

        public virtual async Task<RequestResult> POST(string url, HttpContent postData, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            return await SendRequest(HttpMethod.Post, url, token, postData, requestTuneAction).ConfigureAwait(false);
        }

        public virtual async Task<RequestResult> PUT(string url, HttpContent postData, CancellationToken? token = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            return await SendRequest(HttpMethod.Put, url, token, postData, requestTuneAction).ConfigureAwait(false);
        }

        public virtual async Task<RequestResult> DELETE(string url, CancellationToken? token = null, HttpContent postData = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            return await SendRequest(HttpMethod.Delete, url, token, postData, requestTuneAction).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

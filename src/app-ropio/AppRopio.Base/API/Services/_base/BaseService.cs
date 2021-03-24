using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.API.Helpers;
using AppRopio.Base.API.Models;
using MvvmCross;
using MvvmCross.Platform.Platform;
using Newtonsoft.Json;

namespace AppRopio.Base.API.Services
{
    /// <summary>
    /// Базовый класс для сервисов API
    /// </summary>
    public abstract class BaseService
    {
        private static readonly Stopwatch _stopWatch = new Stopwatch();

        private Lazy<IConnectionService> _lazyConnectionService;
        private Lazy<IMvxTrace> _lazyTrace;

        private IConnectionService _connectionService;
        protected IConnectionService ConnectionService => _lazyConnectionService.Value;

        private IMvxTrace _trace;
        protected IMvxTrace Trace => _lazyTrace.Value;

        protected BaseService()
        {
            _lazyConnectionService = new Lazy<IConnectionService>(() => _connectionService ?? Mvx.Resolve<IConnectionService>());
            _lazyTrace = new Lazy<IMvxTrace>(() => _trace ?? Mvx.Resolve<IMvxTrace>());
        }

        protected BaseService(IConnectionService connectionService, IMvxTrace trace) : this()
        {
            _connectionService = connectionService;
            _trace = trace;
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

        protected async Task WriteInOutput(string url, RequestResult result, Stopwatch stopwatch, HttpContent postData = null)
        {
            var rawContent = result.ResponseCode == System.Net.HttpStatusCode.NoContent ? string.Empty : (result.Exception == null ? (await ReadContentAsString(result)) : result.Exception.Message);

            var responseMessage = DecodeEncodedNonAsciiCharacters(rawContent);

            string formatedOutput = string.Empty;
            formatedOutput += $"{Environment.NewLine}{Environment.NewLine}";
            formatedOutput += $"# REQUEST {Environment.NewLine}";

            var requestHeaders = ConnectionService.Headers;
            var responseHeaders = result.Headers;

            formatedOutput += $"## Request headers {Environment.NewLine}```{Environment.NewLine}{JsonConvert.SerializeObject(requestHeaders)}{Environment.NewLine}```{Environment.NewLine}";

            formatedOutput += $"## Url{Environment.NewLine}`{url}`{Environment.NewLine}";

            formatedOutput += $"## Status code{Environment.NewLine}`{(int)result.ResponseCode} {result.ResponseCode}`{Environment.NewLine}";

            if (postData != null)
            {
                var json = await postData.ReadAsStringAsync();
                formatedOutput += $"## Request message{Environment.NewLine}`{json}`{Environment.NewLine}";
            }
            formatedOutput += $"## Time{Environment.NewLine}`{stopwatch.ElapsedMilliseconds} ms`{Environment.NewLine}";
            formatedOutput += $"# RESPONSE{Environment.NewLine}";
            formatedOutput += $"## Response length{Environment.NewLine}`{(responseMessage == null ? 0 : responseMessage.Length)}`{Environment.NewLine}";
            formatedOutput += $"## Response headers{Environment.NewLine}```{Environment.NewLine}{JsonConvert.SerializeObject(responseHeaders)}{Environment.NewLine}```{Environment.NewLine}";
            formatedOutput += $"## Response message{Environment.NewLine}```{Environment.NewLine}{responseMessage}{Environment.NewLine}```{Environment.NewLine}{Environment.NewLine}";

            Trace.Trace(MvxTraceLevel.Diagnostic, $"{GetType().FullName}", formatedOutput);
        }

        /// <summary>
        /// Конвертация объекта в StringContent посредством сериализации в JSON
        /// </summary>
        /// <returns>StringContent</returns>
        /// <param name="data">Данные для конвертации</param>
        protected StringContent ToStringContent(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data) ?? string.Empty, System.Text.Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Посылает GET запрос на определенный url
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationTokenSource">Cancellation token source.</param>
        public virtual async Task<TModel> Get<TModel>(string url, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
            where TModel : class
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.GET(
                url,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch));

            if (result.Succeeded)
            {
                var parseResult = await result.Parse<TModel>(MediaTypeFormat.Json);
                if (parseResult.Successful)
                    return parseResult.ParsedObject;

                throw new Exception(parseResult.Error);
            }

            throw new ConnectionException(result);
        }

        /// <summary>
        /// Посылает GET запрос на определенный url
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationToken">Cancellation token source.</param>
        public virtual async Task<TModel> Get<TModel>(string url, Func<string, TModel> parseResult, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.GET(
                url,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch));

            if (result.Succeeded)
            {
                var contentAsString = await ReadContentAsString(result);
                return parseResult.Invoke(contentAsString);
            }

            throw new ConnectionException(result);
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

        /// <summary>
        /// Посылает POST запрос на определенный url с передачей postData
        /// </summary>
        /// <param name="postData">POST data.</param>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationToken">Cancellation token source.</param>
        public virtual async Task<TModel> Post<TModel>(string url, HttpContent postData, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
            where TModel : class
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            if (postData == null)
                throw new ArgumentNullException(nameof(postData));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.POST(
                url,
                postData,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch, postData));

            if (result.Succeeded)
            {
                var parseResult = await result.Parse<TModel>(MediaTypeFormat.Json);
                if (parseResult.Successful)
                    return parseResult.ParsedObject;

                throw new Exception(parseResult.Error);
            }

            throw new ConnectionException(result);
        }

        /// <summary>
        /// Посылает POST запрос на определенный url с передачей postData
        /// </summary>
        /// <param name="postData">POST data.</param>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationToken">Cancellation token source.</param>
        public virtual async Task<TModel> Post<TModel>(string url, HttpContent postData, Func<string, TModel> parseResult, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            if (postData == null)
                throw new ArgumentNullException(nameof(postData));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.POST(
                url,
                postData,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch, postData));

            if (result.Succeeded)
            {
                var contentAsString = await ReadContentAsString(result);
                return parseResult.Invoke(contentAsString);
            }

            throw new ConnectionException(result);
        }

        /// <summary>
        /// Посылает GET запрос на определенный url
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationToken">Cancellation token source.</param>
        public virtual async Task Get(string url, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.GET(
                url,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }

        /// <summary>
        /// Посылает POST запрос на определенный url с передачей postData
        /// </summary>
        /// <param name="postData">POST data.</param>
        /// <param name="url">URL.</param>
        /// <param name="errorMessage">Сообщение об ошибке, в случае проблем с получением данных.</param>
        /// <param name="cancellationToken">Cancellation token source.</param>
        public virtual async Task Post(string url, HttpContent postData, string errorMessage = null, CancellationToken? cancellationToken = null, Action<HttpRequestMessage> requestTuneAction = null)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException(nameof(url));
            if (postData == null)
                throw new ArgumentNullException(nameof(postData));

            var stopWatch = Stopwatch.StartNew();

            var result = await ConnectionService.POST(
                url,
                postData,
                cancellationToken,
                requestTuneAction
            );

            stopWatch.Stop();

            Task.Run(() => WriteInOutput(url, result, stopWatch, postData));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }
    }
}


using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Webkit;
using AppRopio.Payments.Core.Services;
using MvvmCross.Binding.Extensions;

namespace AppRopio.Payments.CloudPayments.Droid.Services.Implementation
{
    public class CloudPayments3DSService : IPayment3DSService
    {
        private WebView _webView;

      // Pattern to find the MD value in the ACS server post response
        private const string mdPattern = ".*?(<input[^<>]* name=\\\"MD\\\"[^<>]*>).*?";

        /*
         * Pattern to find the PaRes value in the ACS server post response
         */
        private const string paresPattern= ".*?(<input[^<>]* name=\\\"PaRes\\\"[^<>]*>).*?";

        private const string valuePattern = ".*? value=\\\"(.*?)\\\"";

        private TaskCompletionSource<Dictionary<string, string>> _tcs;

        public void SetWebView(object webView)
        {
            _webView = webView as WebView;

            _webView.Settings.JavaScriptEnabled = true;
            _webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            _webView.Settings.AllowContentAccess = true;
        }

        public Task<Dictionary<string, string>> Process3DS(string url, string redirectUrl, Dictionary<string, string> parameters)
        {
            _tcs = new TaskCompletionSource<Dictionary<string, string>>();

            _webView.AddJavascriptInterface(new PaymentsJSInterface { CompleteAuthorization = OnCompleteAuth }, nameof(PaymentsJSInterface));

            var webViewClient = new CloudPaymentsWebViewClient(redirectUrl);
            webViewClient.OnLoadingError = errorDescription => _tcs.TrySetException(new Exception(errorDescription));
            _webView.SetWebViewClient(webViewClient);

            var webChromeClient = new CloudPaymentsWebChromeClient();
            _webView.SetWebChromeClient(webChromeClient);

            NavigateTo3DSPage(url, parameters);

            return _tcs.Task;
        }

        private void OnCompleteAuth(string html)
        {
            var md = string.Empty;
            var pares = string.Empty;

            md = GetHtmlValue(html, mdPattern);

            pares = GetHtmlValue(html, paresPattern);
        
            var parameters = new Dictionary<string, string>
            {
                ["MD"] = md,
                ["PaRes"] = pares
            };

            if (!_tcs.Task.IsCanceled && !_tcs.Task.IsCompleted)
                _tcs.TrySetResult(parameters);
        }

        private string GetHtmlValue(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            if (match.Success)
            {
                input = (match.Groups.ElementAt(1) as Capture).Value;

                if (!input.IsNullOrEmtpy())
                {
                    var valueMatch = Regex.Match(input, valuePattern);
                    if (valueMatch.Success)
                    {
                        input = (valueMatch.Groups.ElementAt(1) as Capture).Value;
                    }
                }
            }

            return input;
        }

        private void NavigateTo3DSPage(string threeDsUrl, Dictionary<string, string> parameters)
        {
            Android.App.Application.SynchronizationContext.Post(_ =>
            {
                _webView.Visibility = Android.Views.ViewStates.Visible;
            }, null);

            var sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<body>");

            sb.AppendLine($"<form name=\"downloadForm\" action=\"{threeDsUrl}\" method=\"POST\">");
            foreach (var pair in parameters)
                sb.AppendLine($"<input type=\"hidden\" name=\"{pair.Key}\" value=\"{pair.Value}\">");
            sb.AppendLine("</form>");

            sb.AppendLine("<script>");
            sb.AppendLine("window.onload=submitForm;");
            sb.AppendLine("function submitForm() { downloadForm.submit(); }");
            sb.AppendLine("</script>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            var content = sb.ToString();

            Android.App.Application.SynchronizationContext.Post(_ =>
            {
                _webView.LoadDataWithBaseURL(null, content, "text/html; charset=utf-8", "UTF-8", null);
            }, null);
        }

        private class CloudPaymentsWebViewClient : WebViewClient
        {
            private readonly string _redirectUri;

            private bool _handled;

            public Action<string> OnLoadingError { get; set; }

            public CloudPaymentsWebViewClient(string redirectUri)
            {
                _redirectUri = redirectUri;
            }

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                if (request.Url.ToString() == _redirectUri)
                {
                    view.LoadUrl($"javascript:window.{nameof(PaymentsJSInterface)}.processHTML(document.getElementsByTagName('html')[0].innerHTML);");

                    return true;
                }

                return base.ShouldOverrideUrlLoading(view, request);
            }

            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                if (url == _redirectUri)
                {
                    view.LoadUrl($"javascript:window.{nameof(PaymentsJSInterface)}.processHTML(document.getElementsByTagName('html')[0].innerHTML);");

                    return true;
                }

                return base.ShouldOverrideUrlLoading(view, url);
            }

            public override WebResourceResponse ShouldInterceptRequest(WebView view, IWebResourceRequest request)
            {
                var result = request.Url.ToString() == _redirectUri;
                if (result)
                {
                    _handled = true;
                    Android.App.Application.SynchronizationContext.Post(_ =>
                    {
                        view.LoadUrl($"javascript:window.{nameof(PaymentsJSInterface)}.processHTML(document.getElementsByTagName('html')[0].innerHTML);");
                    }, null);

                    return new WebResourceResponse("text/plain", "utf-8", new System.IO.MemoryStream());
                }
                else if (_handled)
                    return new WebResourceResponse("text/plain", "utf-8", new System.IO.MemoryStream());

                return base.ShouldInterceptRequest(view, request);
            }

            public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
            {
                if (request.Url.ToString() != _redirectUri)
                    OnLoadingError?.Invoke(error.Description);
            }

            public override void OnReceivedError(WebView view, [GeneratedEnum] ClientError errorCode, string description, string failingUrl)
            {
                if (failingUrl != _redirectUri)
                    OnLoadingError?.Invoke(description);
            }
        }

        private class CloudPaymentsWebChromeClient : WebChromeClient
        {
        }

        private class PaymentsJSInterface : Java.Lang.Object
        {
            public Action<string> CompleteAuthorization { get; set; }

            [JavascriptInterface, Java.Interop.Export("processHTML")]
            public void processHTML(string paramString)
            {
                CompleteAuthorization?.Invoke(paramString);
            }
        }
    }
}

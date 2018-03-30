using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AppRopio.Payments.Core.Services;
using Foundation;
using UIKit;

namespace AppRopio.Payments.CloudPayments.iOS.Services.Implementation
{
    public class CloudPayments3DSService : IPayment3DSService
    {
        private UIWebView _webView;
        private string _3dsUrl;
        private string _redirectUrl;
        private HttpContent _postContent;

        private TaskCompletionSource<Dictionary<string, string>> _tcs;

        public void SetWebView(object webView)
        {
            _webView = (UIWebView)webView;
			_webView.ShouldStartLoad = ShoulStartLoad;
        }

        public Task<Dictionary<string, string>> Process3DS(string url, string redirectUrl, HttpContent postContent)
        {
            _tcs = new TaskCompletionSource<Dictionary<string, string>>();

            _3dsUrl = url;
            _postContent = postContent;
            _redirectUrl = redirectUrl;

            NavigateTo3DSPage();

            return _tcs.Task;
        }

        private void NavigateTo3DSPage()
        {
            _webView.Hidden = false;
			_webView.LoadError -= WebView_LoadError;
			_webView.LoadError += WebView_LoadError;

            var request = new NSMutableUrlRequest(new NSUrl(_3dsUrl));
			request.HttpMethod = "POST";
			request.Body = NSData.FromString(_postContent.ReadAsStringAsync().Result);
			request["Content-Length"] = request.Body.Length.ToString();

			foreach (var kv in _postContent.Headers)
			{
				request[kv.Key] = string.Join(" ", kv.Value);
			}

			_webView.LoadRequest(request);
        }

		private void WebView_LoadError(object sender, UIWebErrorArgs e)
		{
			_webView.LoadError -= WebView_LoadError;

            _tcs.SetException(new NSErrorException(e.Error));
		}

		private bool ShoulStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            if (request.Url.AbsoluteString == _redirectUrl)
            {
                _webView.Hidden = true;

                var parameters = request.Body.ToString().ToParamsDictionary();
                foreach (var key in parameters.Keys.ToList())
                {
                    parameters[key] = Uri.UnescapeDataString(parameters[key]);
                }

				_webView.LoadError -= WebView_LoadError;

                if (!_tcs.Task.IsCanceled && !_tcs.Task.IsCompleted)
                    _tcs.TrySetResult(parameters);

                return false;
            }

            return true;
        }
    }
}
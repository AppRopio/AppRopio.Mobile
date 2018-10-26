using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppRopio.Payments.Core.Services;
using Android.Webkit;

namespace AppRopio.Payments.CloudPayments.Droid.Services.Implementation
{
    public class CloudPayments3DSService : IPayment3DSService
    {
        private WebView _webView;
        private string _3dsUrl;
        private string _redirectUrl;
        private HttpContent _postContent;

        private TaskCompletionSource<Dictionary<string, string>> _tcs;

        public void SetWebView(object webView)
        {
            _webView = webView as WebView;
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
            
        }
    }
}

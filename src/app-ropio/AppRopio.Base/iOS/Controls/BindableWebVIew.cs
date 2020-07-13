using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using MvvmCross.Core.ViewModels;
using UIKit;
using WebKit;

namespace AppRopio.Base.iOS
{
    [Register("BindableWebView"), DesignTimeVisible(true)]
    public class BindableWebView : WKWebView
    {
        public IMvxCommand<string> ShouldLoadCommand { get; set; }

        public ICommand LoadFinishedCommand { get; set; }

        public delegate bool WKWebLoaderControl(WKWebView webView, NSUrlRequest request);

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (!string.IsNullOrEmpty(value))
                    LoadHtmlString(value, new NSUrl(Path.Combine(NSBundle.MainBundle.BundlePath, "Content/"), true));
            }
        }

        private HttpContent _httpContent;
        public HttpContent HttpContent
        {
            get
            {
                return _httpContent;
            }
            set
            {
                _httpContent = value;
            }
        }

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                if (!string.IsNullOrEmpty(value))
                {
                    var request = new NSMutableUrlRequest(new NSUrl(value));

                    if (HttpContent != null)
                    {
                        request.HttpMethod = "POST";
                        request.Body = NSData.FromString(_httpContent.ReadAsStringAsync().Result);
                        request["Content-Length"] = request.Body.Length.ToString();
                        request["Content-Type"] = _httpContent.Headers.ContentType.ToString();
                    }

                    LoadRequest(request); 
                }
            }
        }

        public WKWebLoaderControl ShouldStartLoad {
            get;
            set;
        }

        public event EventHandler LoadFinished;
        public event EventHandler<WKWebErrorArgs> LoadError;

        public BindableWebView(IntPtr handle)
            : base (handle)
        {
            NavigationDelegate = new BindableNavigationDelegate();
            LoadFinished += HandleLoadFinished;
            ShouldStartLoad = OnShouldStartLoad;
        }

        public BindableWebView(CGRect frame, WKWebViewConfiguration configuration = null)
            : base(frame, configuration ?? new WKWebViewConfiguration() {
                AllowsInlineMediaPlayback = true,
                DataDetectorTypes = WKDataDetectorTypes.All
            })
        {
            NavigationDelegate = new BindableNavigationDelegate();
            LoadFinished += HandleLoadFinished;
        }

        private bool OnShouldStartLoad(WKWebView webView, NSUrlRequest request)
        {
            return ShouldLoadCommand != null ? ShouldLoadCommand.CanExecute(request.Url.AbsoluteString) : true;
        }

        private void HandleLoadFinished(object sender, EventArgs e)
        {
            var url = Url.ToString();

            var command = this.LoadFinishedCommand;
            if (command != null && command.CanExecute(url))
                command.Execute(url);
        }

        private class BindableNavigationDelegate : WKNavigationDelegate
        {
            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                (webView as BindableWebView).LoadFinished?.Invoke(webView, EventArgs.Empty);
            }

            public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
                {
                (webView as BindableWebView).LoadError?.Invoke(webView, new WKWebErrorArgs(error));
            }

            public override void DecidePolicy(WKWebView webView, WKNavigationAction navigationAction, Action<WKNavigationActionPolicy> decisionHandler)
            {
                var bindableWebView = webView as BindableWebView;
                bool? shouldStart = bindableWebView?.ShouldStartLoad(webView, navigationAction.Request);

                if (shouldStart != null && !shouldStart.Value) {
                    decisionHandler?.Invoke(WKNavigationActionPolicy.Cancel);
                    return;
                }

                if (navigationAction.NavigationType == WKNavigationType.LinkActivated) {
                    var url = navigationAction.Request.Url;
                    UIApplication.SharedApplication.OpenUrl(url);
                    decisionHandler?.Invoke(WKNavigationActionPolicy.Cancel);
                } else {
                    decisionHandler?.Invoke(WKNavigationActionPolicy.Allow);
                }
            }
        }
    }

    public class WKWebErrorArgs : EventArgs
    {
        public NSError Error {
            get;
            private set;
        }

        public WKWebErrorArgs(NSError error) {
            Error = error;
        }
    }
}

using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows.Input;
using Foundation;
using MvvmCross.Core.ViewModels;
using UIKit;

namespace AppRopio.Base.iOS
{
    [Register("BindableWebView"), DesignTimeVisible(true)]
    public class BindableWebView : UIWebView
    {
        public IMvxCommand<string> ShouldLoadCommand { get; set; }

        public ICommand LoadFinishedCommand { get; set; }

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

        public BindableWebView(IntPtr handle)
            : base (handle)
        {
            LoadFinished += HandleLoadFinished;
            ShouldStartLoad = OnShouldStartLoad;
        }

        public BindableWebView()
        {
            LoadFinished += HandleLoadFinished;
            ShouldStartLoad = OnShouldStartLoad;
        }

        private bool OnShouldStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            return ShouldLoadCommand != null ? ShouldLoadCommand.CanExecute(request.Url.AbsoluteString) : true;
        }

        private void HandleLoadFinished(object sender, EventArgs e)
        {
            var url = Request.Url.ToString();

            var command = this.LoadFinishedCommand;
            if (command != null && command.CanExecute(url))
                command.Execute(url);
        }
    }
}

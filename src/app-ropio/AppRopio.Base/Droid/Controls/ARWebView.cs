using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Webkit;
using Android.Support.CustomTabs;
using Android.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Android.Graphics;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARWebView")]
    public class ARWebView : WebView
    {
        private string _html;
        private ARWebChromeClient _chromeClient;

        public string Html
        {
            get
            {
                return _html;
            }
            set
            {
                _html = value;
                Application.SynchronizationContext.Post(_ =>
                {
                    this.LoadDataWithBaseURL(null, value, "text/html; charset=utf-8", "UTF-8", null);
                }, null);

            }
        }

        public IMvxCommand<string> ShouldLoadCommand { get; set; }

        private string _urlSource;
        public string UrlSource
        {
            get
            {
                return _urlSource;
            }
            set
            {
                _urlSource = value;

                SetWebViewClient(new ARWebViewClient((DownloadEventArgs arg) =>
                {
                    return ShouldLoadCommand != null ? ShouldLoadCommand.CanExecute(arg.Url) : false;
                }));

                Application.SynchronizationContext.Post(_ =>
                {
                    this.LoadUrl(_urlSource);
                }, null);
            }
        }

        private string _videoUrl;
        public string VideoUrl
        {
            get
            {
                return _videoUrl;
            }
            set
            {
                _videoUrl = value;
                SetWebUrl(value);
            }
        }

        public float AspectRatio { get; set; } = 9.0f / 16.0f;

        private void SetWebUrl(string value)
        {
            Settings.JavaScriptCanOpenWindowsAutomatically = true;
            Settings.AllowFileAccess = true;
            Settings.SetAppCacheEnabled(true);
            Settings.DomStorageEnabled = true;
            Settings.LoadWithOverviewMode = true;
            Settings.UseWideViewPort = true;
            Settings.SetPluginState(WebSettings.PluginState.On);

            var wm = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.WindowManager;
            var display = wm.DefaultDisplay;
            var size = new Point();
            display.GetSize(size);

            string data = "";
            data += "<html>";
            data += "<style> body{ margin: auto; max-width: 100%; max-height: 100%; } </style>";
            data += $"<body><iframe src=\"{value}\" width=\"{size.X}\" height=\"{size.X * AspectRatio}\" webkitallowfullscreen=\"true\" mozallowfullscreen=\"true\" allowfullscreen=\"true\" frameborder=\"0\"></iframe>";
            data += "</body></html>";
            Application.SynchronizationContext.Post(_ =>
            {
                this.LoadDataWithBaseURL(null, data, "text/html; charset=utf-8", "UTF-8", null);
            }, null);
        }

        public Action<View, WebChromeClient.ICustomViewCallback> OnShowCustomView { get; set; }

        public Action OnHideCustomView { get; set; }

        public ARWebView(Context context)
            : base(context)
        {
            Initialize();
        }

        public ARWebView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Initialize();
        }

        public ARWebView(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public ARWebView(Context context, IAttributeSet attrs, int defStyleAttr, bool privateBrowsing)
            : base(context, attrs, defStyleAttr, privateBrowsing)
        {
            Initialize();
        }

        public ARWebView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected ARWebView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        void Initialize()
        {
            var webViewClient = new ARWebViewClient((e) =>
            {
                var tabsBuilder = new CustomTabsIntent.Builder();
                tabsBuilder.SetShowTitle(false);

                var intent = tabsBuilder.Build();
                intent.LaunchUrl(this.Context, Android.Net.Uri.Parse(e.Url));

                return true;
            });
            SetWebViewClient(webViewClient);

            _chromeClient = new ARWebChromeClient();
            _chromeClient.ShowCustomView = (arg1, arg2) =>
            {
                this.Visibility = ViewStates.Gone;
                OnShowCustomView?.Invoke(arg1, arg2);
            };
            _chromeClient.HideCustomView = () =>
            {
                this.Visibility = ViewStates.Visible;
                OnHideCustomView?.Invoke();
            };

            SetWebChromeClient(_chromeClient);

            Settings.JavaScriptEnabled = true;
        }

        public void HideCustomView()
        {
            _chromeClient?.HideCustomView();
        }

        private class ARWebViewClient : WebViewClient
        {
            readonly Func<DownloadEventArgs, bool> callback;

            public ARWebViewClient(Func<DownloadEventArgs, bool> callback)
            {
                this.callback = callback;
            }

#pragma warning disable CS0618 // Type or member is obsolete
#pragma warning disable CS0672 // Member overrides obsolete member
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                return callback?.Invoke(new DownloadEventArgs(url, null, null, null, 0)) ?? base.ShouldOverrideUrlLoading(view, url);
            }
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CS0672 // Member overrides obsolete member

            public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
            {
                return callback?.Invoke(new DownloadEventArgs(request.Url.ToString(), null, null, null, 0)) ?? base.ShouldOverrideUrlLoading(view, request);
            }
        }

        // http://stackoverflow.com/questions/15796661/android-webview-app-wont-let-video-player-go-full-screen/16199649#16199649
        private class ARWebChromeClient : WebChromeClient
        {
            public Action<View, WebChromeClient.ICustomViewCallback> ShowCustomView { get; set; }
            public Action HideCustomView { get; set; }

            public override void OnShowCustomView(View view, WebChromeClient.ICustomViewCallback callback)
            {
                ShowCustomView?.Invoke(view, callback);
            }

            public override void OnHideCustomView()
            {
                HideCustomView?.Invoke();
            }
        }
    }
}

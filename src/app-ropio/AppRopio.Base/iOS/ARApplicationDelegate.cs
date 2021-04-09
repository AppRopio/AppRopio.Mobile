using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.Push;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross;
using MvvmCross.Core;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;
using UserNotifications;
using WebKit;

namespace AppRopio.Base.iOS
{
    public abstract class ARApplicationDelegate : MvxApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        private TaskCompletionSource<bool> _loaderLoadedTCS;

        #region Properties

        protected NSDictionary NotificationUserInfo { get; private set; }

        protected Action<NSDictionary> NotificationScheduled;

        protected Action OnInitialized;

        protected bool Initialized { get; private set; }

        protected bool Activated { get; private set; }

        public override UIWindow Window { get; set; }

        #endregion

        #region Services

        protected IAnalyticsNotifyingService AnalyticsNotifyingService 
        {
            get
            {
                try
                {
                    return Mvx.IoCProvider.Resolve<IAnalyticsNotifyingService>();
                }
                catch
                {
                    return new AnalyticsNotifyingService();
                }
            }
        }

        #endregion

        #region Lifecycle

        #region Init

        protected virtual UIViewController ConstructDefaultViCo()
        {
            _loaderLoadedTCS = new TaskCompletionSource<bool>();

            var viewController = new UIViewController();
            viewController.View.BackgroundColor = (UIColor)Theme.ColorPalette.Accent;

            var webView = new BindableWebView(new CoreGraphics.CGRect(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenHeight), new WKWebViewConfiguration() {
                AllowsInlineMediaPlayback = true,
                DataDetectorTypes = WKDataDetectorTypes.All
            });
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Clear;
            webView.ScrollView.BackgroundColor = UIColor.Clear;
            webView.ScrollView.ScrollEnabled = false;
            webView.ScrollView.UserInteractionEnabled = false;
            webView.UserInteractionEnabled = false;
            webView.LoadFinished += (sender, ev) =>
            {
                _loaderLoadedTCS.TrySetResult(true);
            };

            var path = NSBundle.MainBundle.GetUrlForResource("loader", "html");
            webView.LoadFileUrl(path, path);

            viewController.View.AddSubview(webView);

            return viewController;
        }

        protected override void RunAppStart(object hint = null)
        {
            if (Initialized)
                return;

            MvxIosSetupSingleton.Instance.PlatformSetup<MvxIosSetup>().StateChanged += (sender, ev) =>
            {
                if (ev.SetupState != MvxSetup.MvxSetupState.Initialized)
                    return;

                Mvx.IoCProvider.CallbackWhenRegistered<IMvxAppStart>(startup =>
                {
                    if (!startup.IsStarted)
                    {
                        Task.Run(async () =>
                        {
                            await startup.StartAsync(GetAppStartHint(hint));

                            InvokeOnMainThread(() =>
                            {
                                RequestNotificationAuthorization();

                                Initialized = true;

                                OnInitialized?.Invoke();
                            });
#if DEBUG
                            System.Diagnostics.Debug.WriteLine($"Ellapsed milliseconds after MvvmCross setup {Base.Core.ServicesDebug.StartupTimerService.Instance.EllapsedMilliseconds()}");
                            Base.Core.ServicesDebug.StartupTimerService.Instance.StopTimer();
#endif
                        });
                    }
                });
            };
        }

        #endregion

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            Window.RootViewController = ConstructDefaultViCo();

            Window.MakeKeyAndVisible();

            Task.Run(async () =>
            {
                if (_loaderLoadedTCS != null)
                    await _loaderLoadedTCS.Task;

                base.FinishedLaunching(application, launchOptions);
            })
            .ContinueWith((task) =>
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(task.Exception);
#endif
            }, TaskContinuationOptions.None);
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"AppRopio.Test.iOS.AppDelegate:Diagnostic: Ellapsed milliseconds in the end of FinishedLaunching {Base.Core.ServicesDebug.StartupTimerService.Instance.EllapsedMilliseconds()}");
#endif
            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            Activated = true;

            NotificationScheduled?.Invoke(NotificationUserInfo);
            NotificationScheduled = null;

            AnalyticsNotifyingService?.NotifyApp(Core.Models.Analytics.AppState.Foreground);
        }

        public override void DidEnterBackground(UIApplication application)
        {
            Activated = false;

            AnalyticsNotifyingService?.NotifyApp(Core.Models.Analytics.AppState.Background);

            base.DidEnterBackground(application);
        }

        public override void WillTerminate(UIApplication application)
        {
            Activated = false;

            AnalyticsNotifyingService?.NotifyApp(Core.Models.Analytics.AppState.Finished);

            base.WillTerminate(application);
        }

        #endregion

        #region Push notifications

        //LINK: https://developer.xamarin.com/guides/ios/platform_features/introduction-to-ios10/user-notifications/enhanced-user-notifications/#About-Remote-Notifications

        protected virtual void RequestNotificationAuthorization()
        {
            UNUserNotificationCenter.Current.Delegate = this;

            var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            {
                if (granted)
                    InvokeOnMainThread(() => UIApplication.SharedApplication.RegisterForRemoteNotifications());
            });
        }

        protected virtual void OnNotificationTapped(NSDictionary userInfo)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

            try
            {
                var deeplinkKey = new NSString("deeplink");
                if (userInfo.ContainsKey(deeplinkKey))
                {
                    var deeplink = userInfo.ValueForKey(deeplinkKey).ToString();

                    if (Mvx.IoCProvider.CanResolve<IViewModelLookupService>())
                        NavigateToDeeplinkFromPush(userInfo, deeplink);
                    else
                        Mvx.IoCProvider.CallbackWhenRegistered<IViewModelLookupService>(() => NavigateToDeeplinkFromPush(userInfo, deeplink));
                }
            }
            catch { }
        }

        protected virtual void NavigateToDeeplinkFromPush(NSDictionary userInfo, string deeplink)
        {
            var vmLS = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            if (vmLS.IsRegisteredDeeplink(deeplink))
            {
                Mvx.IoCProvider.CallbackWhenRegistered<IPushNotificationsService>(() => Mvx.IoCProvider.Resolve<IPushNotificationsService>().NavigateTo(deeplink));
            }
            else if (!Initialized)
            {
                OnInitialized = () =>
                    {
                        vmLS.CallbackWhenDeeplinkRegistered(deeplink, type =>
                        {
                            InvokeOnMainThread(() => Mvx.IoCProvider.CallbackWhenRegistered<IPushNotificationsService>(() => Mvx.IoCProvider.Resolve<IPushNotificationsService>().NavigateTo(deeplink)));
                        });
                    };
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            byte[] result = new byte[deviceToken.Length];
            Marshal.Copy(deviceToken.Bytes, result, 0, (int)deviceToken.Length);
            var token = BitConverter.ToString(result).Replace("-", "");

            Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"\nPush token: {token}\n");

            Mvx.IoCProvider.CallbackWhenRegistered<IPushNotificationsService>(async () =>
            {
                try
                {
                    AppSettings.PushToken = token;
                    await Mvx.IoCProvider.Resolve<IPushNotificationsService>().RegisterDeviceForPushNotificatons(token);
                }
                catch { }
            });
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"\nRegister for remote notifications failed: {error.ToString()}\n");
        }

        //Handling Action Responses
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public virtual void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            var userInfo = response.Notification.Request.Content.UserInfo;

            if (Activated)
                OnNotificationTapped(userInfo);
            else
            {
                NotificationUserInfo = userInfo;
                NotificationScheduled += OnNotificationTapped;
            }

            completionHandler();
        }

        //Handling Foreground App Notifications
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public virtual void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                OnNotificationTapped(userInfo);
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Inactive)
                OnNotificationTapped(userInfo);
            if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Background)
                OnNotificationTapped(userInfo);

            completionHandler(UIBackgroundFetchResult.NoData);
        }

        #endregion
    }
}

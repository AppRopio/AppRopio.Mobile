using System;
using System.Threading.Tasks;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.Push;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using UIKit;
using UserNotifications;

namespace AppRopio.Base.iOS
{
    public abstract class ARApplicationDelegate : MvxApplicationDelegate, IUNUserNotificationCenterDelegate
    {
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
                    return Mvx.Resolve<IAnalyticsNotifyingService>();
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

        protected abstract MvxAsyncIosSetup CreateSetup(IMvxApplicationDelegate appDelegate, MvxIosViewPresenter presenter);

        protected abstract MvxIosViewPresenter CreatePresenter(IMvxApplicationDelegate appDelegate, UIWindow window);

        protected virtual UIWindow CreateWindow()
        {
            return new UIWindow(UIScreen.MainScreen.Bounds);
        }

        protected virtual UIViewController ConstructDefaultViCo()
        {
            var viewController = new UIViewController();
            viewController.View.BackgroundColor = (UIColor)Theme.ColorPalette.Accent;

            var webView = new UIWebView(new CoreGraphics.CGRect(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenHeight));
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Clear;
            webView.ScrollView.BackgroundColor = UIColor.Clear;
            webView.ScrollView.ScrollEnabled = false;
            webView.ScrollView.UserInteractionEnabled = false;
            webView.UserInteractionEnabled = false;

            var path = NSBundle.MainBundle.PathForResource("loader", "html");
            webView.LoadRequest(NSUrlRequest.FromUrl(NSUrl.FromString(path)));

            viewController.View.AddSubview(webView);

            return viewController;
        }

        protected virtual void StartMvvmCross(MvxIosViewPresenter presenter, MvxAsyncIosSetup setup)
        {
            Task.Run(async () =>
            {
                try
                {
                    await setup.InitializeAsync();

                    var startup = Mvx.Resolve<IMvxAppStart>();

                    InvokeOnMainThread(() =>
                    {
                        startup.Start();

                        RequestNotificationAuthorization();

                        Initialized = true;

                        OnInitialized?.Invoke();
                    });
#if DEBUG
                    MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, this.GetType().FullName, $"Ellapsed milliseconds after MvvmCross setup {Base.Core.ServicesDebug.StartupTimerService.Instance.EllapsedMilliseconds()}");
                    Base.Core.ServicesDebug.StartupTimerService.Instance.StopTimer();
#endif
                }
                catch (Exception ex)
                {
                    MvxTrace.TaggedTrace(MvxTraceLevel.Error, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
                }
            });
        }

        #endregion

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Window = CreateWindow();

            var presenter = CreatePresenter(this, Window);

            var setup = CreateSetup(this, presenter);

            StartMvvmCross(presenter, setup);

            Window.RootViewController = ConstructDefaultViCo();

            Window.MakeKeyAndVisible();
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

                    if (Mvx.CanResolve<IViewModelLookupService>())
                        NavigateToDeeplinkFromPush(userInfo, deeplink);
                    else
                        Mvx.CallbackWhenRegistered<IViewModelLookupService>(() => NavigateToDeeplinkFromPush(userInfo, deeplink));
                }
            }
            catch { }
        }

        protected virtual void NavigateToDeeplinkFromPush(NSDictionary userInfo, string deeplink)
        {
            var vmLS = Mvx.Resolve<IViewModelLookupService>();
            if (vmLS.IsRegisteredDeeplink(deeplink))
            {
                Mvx.CallbackWhenRegistered<IPushNotificationsService>(service => service.NavigateTo(deeplink));
            }
            else if (!Initialized)
            {
                OnInitialized = () =>
                    {
                        vmLS.CallbackWhenDeeplinkRegistered(deeplink, type =>
                        {
                            InvokeOnMainThread(() => Mvx.CallbackWhenRegistered<IPushNotificationsService>(service => service.NavigateTo(deeplink)));
                        });
                    };
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            string token = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(token))
            {
                token = token.Trim('<');
                token = token.Trim('>');
            }
            token = token.Replace(" ", string.Empty);

            MvxTrace.Trace($"\nPush token: {token}\n");

            Mvx.CallbackWhenRegistered<IPushNotificationsService>(async service =>
            {
                try
                {
                    AppSettings.PushToken = token;
                    await service.RegisterDeviceForPushNotificatons(token);
                }
                catch { }
            });
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            MvxTrace.Trace($"\nRegister for remote notifications failed: {error.ToString()}\n");
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

using AppRopio.Base.iOS;
using AppRopio.Navigation.Menu.iOS;
using AppRopio.Navigation.Menu.iOS.Navigation;
using AppRopio_Test.Views;
using Foundation;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using UIKit;

namespace AppRopio.Test.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : ARApplicationDelegate
    {
        public UIWindow DemoWindow { get; private set; }

        protected override MvxAsyncIosSetup CreateSetup(IMvxApplicationDelegate appDelegate, MvxIosViewPresenter presenter)
        {
            return new MenuSetup(appDelegate, presenter);
        }

        protected override MvxIosViewPresenter CreatePresenter(IMvxApplicationDelegate appDelegate, UIWindow window)
        {
            return new MenuNavigationPresenter(appDelegate, window);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            DemoWindow = CreateWindow();
            DemoWindow = new UIWindow(UIScreen.MainScreen.Bounds) 
            {
                RootViewController = new DemoViewController(() => 
                {
                    base.FinishedLaunching(application, launchOptions);
                    DemoWindow.Hidden = true;
                })
            };
            DemoWindow.MakeKeyAndVisible();

            return true;
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation)
                           || VKontakte.VKSdk.ProcessOpenUrl(url, sourceApplication);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var urlOptions = new UIApplicationOpenUrlOptions(options);

            return Facebook.CoreKit.ApplicationDelegate.SharedInstance.OpenUrl(app, url, urlOptions.SourceApplication, urlOptions.Annotation)
                           || VKontakte.VKSdk.ProcessOpenUrl(url, urlOptions.SourceApplication);
        }
    }
}



using AppRopio.Base.iOS;
using AppRopio.Navigation.Menu.iOS;
using AppRopio_Test.Views;
using Foundation;
using UIKit;

namespace AppRopio.Test.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
    public class AppDelegate : ARApplicationDelegate<MenuSetup>
    {
        public UIWindow DemoWindow { get; private set; }

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



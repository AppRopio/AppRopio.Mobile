using Android.App;
using Android.Content.PM;
using AppRopio.Base.Droid.Views;

namespace AppRopio.Test.Droid
{
    [Activity(MainLauncher = true,
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : CommonSplashScreenActivity
    {
        public SplashActivity()
            : base (Resource.Layout.Splash)
        {
            
        }
    }
}

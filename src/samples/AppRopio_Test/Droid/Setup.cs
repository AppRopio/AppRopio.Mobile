using Android.Content;
using AppRopio.Base.Droid.FCM;
using AppRopio.Navigation.Menu.Droid;
using AppRopio.Navigation.Menu.Droid.Views;

namespace AppRopio.Test.Droid
{
    public class Setup : MenuSetup
    {
        public Setup(Context applicationContext) 
            : base(applicationContext)
        {
            FcmSettings.Instance.ColorHex = "#FFD535";
            FcmSettings.Instance.IconResourceId = Droid.Resource.Drawable.logo;
            FcmSettings.Instance.ActivityType = typeof(MenuActivity);
        }
    }
}

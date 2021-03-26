using Xamarin.Essentials;

namespace AppRopio.Navigation.Menu.Core
{
    public static class MenuSettings
    {
        public static bool FirstLaunch
        {
            get
            {
                return Preferences.Get("FirstLaunch", true);
            }
            set
            {
                Preferences.Set("FirstLaunch", value);
            }
        }
    }
}

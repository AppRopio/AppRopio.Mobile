using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppRopio.Navigation.Menu.Core
{
    public static class MenuSettings
    {
        private static ISettings Instance
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static bool FirstLaunch
        {
            get
            {
                return Instance.GetValueOrDefault("FirstLaunch", true);
            }
            set
            {
                Instance.AddOrUpdateValue("FirstLaunch", value);
            }
        }
    }
}

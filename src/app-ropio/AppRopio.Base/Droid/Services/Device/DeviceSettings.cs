using System;
using Xamarin.Essentials;

namespace AppRopio.Base.Droid.Services.Device
{
	public class DeviceSettings
    {
        #region Setting Constants

        private const string Key = "device_token";
        private static readonly string Default = Guid.NewGuid().ToString();

        #endregion


        public static string DeviceToken
        {
            get
            {
                if (!Preferences.ContainsKey(Key)) {
                    Preferences.Set(Key, Default);
                }
                return Preferences.Get(Key, Default);
            }
            set
            {
                Preferences.Set(Key, value);
            }
        }
    }
}

using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppRopio.Base.Droid.Services.Device
{
    public class DeviceSettings
    {
        private static ISettings Instance
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string Key = "device_token";
        private static readonly string Default = new Guid().ToString();

        #endregion


        public static string DeviceToken
        {
            get
            {
                return Instance.GetValueOrDefault(Key, Default);
            }
            set
            {
                Instance.AddOrUpdateValue(Key, value);
            }
        }
    }
}

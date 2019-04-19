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
        private static readonly string Default = Guid.NewGuid().ToString();

        #endregion


        public static string DeviceToken
        {
            get
            {
                if (!Instance.Contains(Key)) {
                    Instance.AddOrUpdateValue(Key, Default);
                }
                return Instance.GetValueOrDefault(Key, Default);
            }
            set
            {
                Instance.AddOrUpdateValue(Key, value);
            }
        }
    }
}

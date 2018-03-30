using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AppRopio.Base.Auth.Core
{
    public static class AuthSettings
    {
        private static ISettings Instance
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string Token
        {
            get { return Instance.GetValueOrDefault(AuthConst.TOKEN_KEY, string.Empty); }
            set { Instance.AddOrUpdateValue(AuthConst.TOKEN_KEY, value); }
        }
    }
}

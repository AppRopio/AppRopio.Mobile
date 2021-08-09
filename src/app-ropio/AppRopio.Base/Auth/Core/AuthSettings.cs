using Xamarin.Essentials;

namespace AppRopio.Base.Auth.Core
{
    public static class AuthSettings
    {
        public static string Token
        {
            get { return Preferences.Get(AuthConst.TOKEN_KEY, string.Empty); }
            set { Preferences.Set(AuthConst.TOKEN_KEY, value); }
        }
    }
}

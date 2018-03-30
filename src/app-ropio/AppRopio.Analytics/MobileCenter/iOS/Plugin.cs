using AppRopio.Analytics.MobileCenter.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Analytics.MobileCenter.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMobileCenter>(new Services.MobileCenter());
        }
    }
}

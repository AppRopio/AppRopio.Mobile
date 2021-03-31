using AppRopio.Analytics.GoogleAnalytics.Core;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.GoogleAnalytics.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IGoogleAnalytics>(() => new Services.GoogleAnalytics());
        }
    }
}

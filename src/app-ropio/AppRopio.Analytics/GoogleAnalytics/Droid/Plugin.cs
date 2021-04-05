using AppRopio.Analytics.GoogleAnalytics.Core;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using AppRopio.Base.Core.Plugins;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.GoogleAnalytics.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "GoogleAnalytics";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IGoogleAnalytics>(() => new Services.GoogleAnalytics());
        }
    }
}

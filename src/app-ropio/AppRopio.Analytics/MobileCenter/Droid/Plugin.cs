using AppRopio.Analytics.MobileCenter.Core;
using AppRopio.Analytics.MobileCenter.Core.Services;
using AppRopio.Base.Core.Plugins;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.MobileCenter.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "MobileCenter";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IMobileCenter>(new Services.MobileCenter());
        }
    }
}

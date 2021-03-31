using AppRopio.Analytics.MobileCenter.Core;
using AppRopio.Analytics.MobileCenter.Core.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.MobileCenter.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IMobileCenter>(new Services.MobileCenter());
        }
    }
}

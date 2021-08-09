using System;
using AppRopio.Base.Core.Plugins;
using AppRopio.Beacons.Core;
using AppRopio.Beacons.Core.Services;
using AppRopio.Beacons.Droid.Services;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Beacons.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Beacons";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IBeaconsScanService>(() => new BeaconsScanService());

            try
            {
                Mvx.IoCProvider.Resolve<IBeaconsScanService>().Start();
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Trace($"Beacons: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }
    }
}

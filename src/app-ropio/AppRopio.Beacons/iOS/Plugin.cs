using System;
using AppRopio.Beacons.Core;
using AppRopio.Beacons.Core.Services;
using AppRopio.Beacons.iOS.Services;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Beacons.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
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

using System;
using AppRopio.Beacons.Core.Services;
using AppRopio.Beacons.Droid.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Beacons.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IBeaconsScanService>(() => new BeaconsScanService());

            try
            {
                Mvx.Resolve<IBeaconsScanService>().Start();
            }
            catch (Exception ex)
            {
                MvxTrace.TaggedTrace("Beacons", ex.BuildAllMessagesAndStackTrace());
            }
        }
    }
}

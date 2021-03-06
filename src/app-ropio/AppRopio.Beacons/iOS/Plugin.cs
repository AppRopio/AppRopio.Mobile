﻿using System;
using MvvmCross.Platform.Plugins;
using AppRopio.Beacons.Core.Services;
using MvvmCross.Platform;
using AppRopio.Beacons.iOS.Services;
using MvvmCross.Platform.Platform;

namespace AppRopio.Beacons.iOS
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

using System;
using System.Threading.Tasks;
using Android.App;
using AppRopio.Beacons.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace AppRopio.Beacons.Droid.Services
{
    public class BeaconsScanService : IBeaconsScanService
    {
        #region Properties

        protected Activity TopActivity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        #endregion

        #region IBeaconsScanService implementation

        public async Task Start()
        {
            (TopActivity.Application as IBeaconApplication).StartRagingBeacons();
        }

        public async Task Stop()
        {
            (TopActivity.Application as IBeaconApplication).StopRagingBeacons();
        }

        #endregion
    }
}

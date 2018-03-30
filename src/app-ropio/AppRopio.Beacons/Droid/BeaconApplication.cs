using System;
using System.Collections.Generic;
using System.Linq;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Powersave;
using AltBeaconOrg.BoundBeacon.Startup;
using Android.App;
using Android.OS;
using Android.Runtime;
using MvvmCross.Platform.Platform;
using Org.Altbeacon.Beacon.Utils;
using AppRopio.Beacons.Core.Services.Implementation;

namespace AppRopio.Beacons.Droid
{
    public interface IBeaconApplication
    {
        void StartRagingBeacons();

        void StopRagingBeacons();
    }

    public class BeaconApplication : Application, IBeaconApplication, Application.IActivityLifecycleCallbacks, IBootstrapNotifier, IBeaconConsumer
    {
        #region Fields

        private RegionBootstrap _regionBootstrap;
        private BackgroundPowerSaver _powerSaver;
        
        private const string TAG = "Beacons";

        protected static bool _haveDetectedBeaconsSinceBoot = false;

        protected static string BEACONS_REGION_HEADER = "beacons_";

        protected static int BEACONS_UPDATES_IN_SECONDS = 5;
        protected static long BEACONS_UPDATES_IN_MILLISECONDS = BEACONS_UPDATES_IN_SECONDS * 1000;

        protected AltBeaconOrg.BoundBeacon.Region _rangingRegion;
        protected RangeNotifier _rangeNotifier;
        protected BeaconManager _beaconManager;

        #endregion

        #region Constructor

        public BeaconApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        #endregion

        #region IBeaconApplication implementation

        public void StartRagingBeacons()
        {
            MvxTrace.TaggedTrace(TAG, "StartRagingBeacons");

            if (_rangeNotifier != null)
                return;

            BEACONS_REGION_HEADER = $"beacons_{Context.PackageName}";

            _beaconManager = BeaconManager.GetInstanceForApplication(this);

            _regionBootstrap = new RegionBootstrap(this, new AltBeaconOrg.BoundBeacon.Region(BEACONS_REGION_HEADER, null, null, null));
            _powerSaver = new BackgroundPowerSaver(this);

            RegisterActivityLifecycleCallbacks(this);

            _rangeNotifier = new RangeNotifier();

            //iBeacon
            _beaconManager.BeaconParsers.Add(new BeaconParser().SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));

            //Estimote
            _beaconManager.BeaconParsers.Add(new BeaconParser().SetBeaconLayout(BeaconParser.EddystoneUidLayout));
            _beaconManager.BeaconParsers.Add(new BeaconParser().SetBeaconLayout(BeaconParser.EddystoneTlmLayout));

            _beaconManager.BeaconParsers.Add(new BeaconParser().SetBeaconLayout(BeaconParser.EddystoneUrlLayout));

            _beaconManager.Bind(this);
        }

        public void StopRagingBeacons()
        {

        }

        #endregion

        #region IActivityLifecycleCallbacks implementation

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);

            if (_beaconManager.IsBound(this))
                _beaconManager.Unbind(this);
        }

        public virtual void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            _haveDetectedBeaconsSinceBoot = true;
        }

        public virtual void OnActivityDestroyed(Activity activity)
        {
            _haveDetectedBeaconsSinceBoot = false;
        }

        public virtual void OnActivityPaused(Activity activity)
        {
            if (_beaconManager.IsBound(this))
                _beaconManager.SetBackgroundMode(true);
        }

        public virtual void OnActivityResumed(Activity activity)
        {
            _haveDetectedBeaconsSinceBoot = true;

            if (_beaconManager.IsBound(this))
                _beaconManager.SetBackgroundMode(false);
        }

        public virtual void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public virtual void OnActivityStarted(Activity activity)
        {
            _haveDetectedBeaconsSinceBoot = true;
        }

        public virtual void OnActivityStopped(Activity activity)
        {
        }

        #endregion

        #region IBootstrapNotifier implementation

        public virtual void DidDetermineStateForRegion(int state, AltBeaconOrg.BoundBeacon.Region region)
        {
            MvxTrace.TaggedTrace(TAG, "DidDetermineStateForRegion {2}. State: {0}, SinceBoot: {1}", state, _haveDetectedBeaconsSinceBoot, region.UniqueId);
        }

        public virtual void DidEnterRegion(AltBeaconOrg.BoundBeacon.Region region)
        {
            MvxTrace.TaggedTrace(TAG, "DidEnterRegion {1}. SinceBoot: {0}", _haveDetectedBeaconsSinceBoot, region.UniqueId);

            if (!_haveDetectedBeaconsSinceBoot)
                _haveDetectedBeaconsSinceBoot = true;
        }

        public virtual void DidExitRegion(AltBeaconOrg.BoundBeacon.Region region)
        {
            MvxTrace.TaggedTrace(TAG, "DidExitRegion");
        }

        #endregion

        #region IBeaconConsumer implementation

        public virtual void OnBeaconServiceConnect()
        {
            _beaconManager.SetForegroundScanPeriod(BEACONS_UPDATES_IN_MILLISECONDS);
            _beaconManager.SetForegroundBetweenScanPeriod(BEACONS_UPDATES_IN_MILLISECONDS);

            _beaconManager.SetBackgroundScanPeriod(BEACONS_UPDATES_IN_MILLISECONDS);
            _beaconManager.SetBackgroundBetweenScanPeriod(BEACONS_UPDATES_IN_MILLISECONDS);

            _beaconManager.UpdateScanPeriods();

            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
            _beaconManager.SetRangeNotifier(_rangeNotifier);

            _rangingRegion = new AltBeaconOrg.BoundBeacon.Region(BEACONS_REGION_HEADER, null, null, null);
            _beaconManager.StartRangingBeaconsInRegion(_rangingRegion);
        }

        #endregion

        #region Beacon

        protected virtual async void RangingBeaconsInRegion(object sender, ICollection<AltBeaconOrg.BoundBeacon.Beacon> beacons)
        {
            MvxTrace.TaggedTrace(TAG, "RangingBeaconsInRegion");

            if (beacons != null && beacons.Count > 0)
            {
                var foundBeacons = beacons.ToList();
                foreach (var beacon in foundBeacons)
                {
                    if (beacon.BeaconTypeCode == 0x10) //Eddystone URL
                    {
                        var url = UrlBeaconUrlCompressor.Uncompress(beacon.Id1.ToByteArray());
                        await BeaconsService.Instance.ActivateEddystoneUrl(url);
                    }
                    else if (beacon.BeaconTypeCode == 0x00) //Eddystone UID
                    {
                        var uid = beacon.Id1.ToString().Replace("0x", "") + beacon.Id2.ToString().Replace("0x", "");
                        await BeaconsService.Instance.ActivateEddystoneUrl(uid);
                    }
                    else if (beacon.BeaconTypeCode == 533) //iBeacon
                    {
                        await BeaconsService.Instance.ActivateiBeacon(
                            beacon.Id1.ToString().Replace("-", ""), 
                            Int32.Parse(beacon.Id2.ToString()), 
                            Int32.Parse(beacon.Id3.ToString())
                        );
                    }
                }
            }

        }

        public class RangeNotifier : Java.Lang.Object, IRangeNotifier
        {
            public event EventHandler<ICollection<AltBeaconOrg.BoundBeacon.Beacon>> DidRangeBeaconsInRegionComplete;

            public void DidRangeBeaconsInRegion(ICollection<AltBeaconOrg.BoundBeacon.Beacon> beacons, AltBeaconOrg.BoundBeacon.Region region)
            {
                DidRangeBeaconsInRegionComplete?.Invoke(this, beacons);
            }
        }

        #endregion
    }
}

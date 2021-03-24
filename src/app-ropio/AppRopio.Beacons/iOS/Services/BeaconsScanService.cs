using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Beacons.Core.Services;
using AppRopio.Beacons.iOS.Models;
using CoreBluetooth;
using CoreFoundation;
using CoreLocation;
using Foundation;
using AppRopio.Beacons.Core.Services.Implementation;
using MvvmCross.Platform.Platform;
using AppRopio.Beacons.iOS.Util;
using Newtonsoft.Json;
using MvvmCross;
using AppRopio.Base.Core.Services.Device;

namespace AppRopio.Beacons.iOS.Services
{
    public class BeaconsScanService : CBCentralManagerDelegate, IBeaconsScanService
    {
        #region Fields

        private static string BEACONS_REGION_HEADER = "beacons_";

        private readonly CLLocationManager _locationMgr;
        private readonly List<CLBeaconRegion> _listOfCLBeaconRegion;

        private static DispatchQueue beacon_operations_queue;
        private double _onLostTimeout = 15.0;
        private bool _shouldBeScanning = false;

        private readonly CBCentralManager _centralManager;

        private NSMutableDictionary _seenEddystoneCache;
        private Dictionary<NSUuid, NSData> _deviceIDCache;

        private Dictionary<string, bool> _regionsCache;

        #endregion

        #region Constructor

        public BeaconsScanService()
        {
            BEACONS_REGION_HEADER = $"beacons_{Mvx.Resolve<IDeviceService>().PackageName}";

            beacon_operations_queue = new DispatchQueue("beacon_operations_queue");

            _deviceIDCache = new Dictionary<NSUuid, NSData>();
            _seenEddystoneCache = new NSMutableDictionary();

            _centralManager = new CBCentralManager(this, beacon_operations_queue);
            _centralManager.Delegate = this;

            _locationMgr = new CLLocationManager();
            _listOfCLBeaconRegion = new List<CLBeaconRegion>();

            _regionsCache = new Dictionary<string, bool>();
        }

        #endregion

        #region IBeaconsScanService implementation

        public async Task Start()
        {
            var result = await BeaconsUtil.RequestPermissionAsync();
            if (!result)
                return;

            _locationMgr.DidRangeBeacons += HandleDidRangeBeacons;
            _locationMgr.DidDetermineState += HandleDidDetermineState;
            _locationMgr.PausesLocationUpdatesAutomatically = false;
            _locationMgr.StartUpdatingLocation();

            beacon_operations_queue.DispatchAsync(StartScanningSynchronized);

            var location = await BeaconsUtil.GetCurrentLocationAsync();
            var ibeacons = await BeaconsService.Instance.LoadBeaconsByUserLocation(location.Coordinate.Latitude, location.Coordinate.Longitude);
            //Начинаем мониторинг
            foreach (var ibeacon in ibeacons)
            {
                var clBeaconRegion = new CLBeaconRegion(new NSUuid(ibeacon.UUID), (ushort)ibeacon.Major, (ushort)ibeacon.Minor, $"{BEACONS_REGION_HEADER}.{ibeacon.ToString()}");
                clBeaconRegion.NotifyEntryStateOnDisplay = true;
                clBeaconRegion.NotifyOnEntry = true;
                clBeaconRegion.NotifyOnExit = true;

                _listOfCLBeaconRegion.Add(clBeaconRegion);

                _locationMgr.StartMonitoring(clBeaconRegion);
                _locationMgr.StartRangingBeacons(clBeaconRegion);

                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", "Start monitoring " + JsonConvert.SerializeObject(ibeacon));
            }
        }

        public async Task Stop()
        {
            _centralManager.StopScan();

            foreach (var beaconRegion in _listOfCLBeaconRegion)
            {
                _locationMgr.StopRangingBeacons(beaconRegion);
                _locationMgr.StopMonitoring(beaconRegion);
            }

            _listOfCLBeaconRegion.Clear();

            _locationMgr.DidRangeBeacons -= HandleDidRangeBeacons;
            _locationMgr.StopUpdatingLocation();
        }

        #endregion

        #region iBeacon monitoring

        private void HandleDidDetermineState(object sender, CLRegionStateDeterminedEventArgs e)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", "HandleDidDetermineState");
            if (e.State == CLRegionState.Inside)
            {
                if (!_regionsCache.ContainsKey(e.Region.Identifier))
                    _regionsCache.Add(e.Region.Identifier, false);

                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", $"Inside region {e.Region.Identifier}");
            }
            else if (e.State == CLRegionState.Outside)
            {
                if (_regionsCache.ContainsKey(e.Region.Identifier))
                    _regionsCache.Remove(e.Region.Identifier);

                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", $"Outside region {e.Region.Identifier}");
            }
        }

        private void HandleDidRangeBeacons(object sender, CLRegionBeaconsRangedEventArgs e)
        {
            if (_regionsCache.ContainsKey(e.Region.Identifier) && !_regionsCache[e.Region.Identifier])
            {
                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", $"Did range region {e.Region.Identifier}");

                _regionsCache[e.Region.Identifier] = true;

                foreach (var beacon in e.Beacons)
                {
                    string uuid = beacon.ProximityUuid.AsString();
                    var major = (int)beacon.Major;
                    var minor = (int)beacon.Minor;

                    SendBeaconChangeProximity(uuid, major, minor);
                }
            }
        }

        private async Task SendBeaconChangeProximity(string uuid, int major, int minor)
        {
            MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", $"Founded Beacon {uuid}:{major}:{minor}");
            await BeaconsService.Instance.ActivateiBeacon(uuid, major, minor);
        }

        #endregion

        #region CBCentralManagerDelegate callbacks

        public override void UpdatedState(CBCentralManager central)
        {
            if (central.State == CBCentralManagerState.PoweredOn && _shouldBeScanning)
                StartScanningSynchronized();
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            if (advertisementData.ContainsKey(CBAdvertisement.DataServiceDataKey))
            {
                var tmp = advertisementData.ObjectForKey(CBAdvertisement.DataManufacturerDataKey);
                var serviceData = advertisementData.ValueForKey(CBAdvertisement.DataServiceDataKey) as NSDictionary;

                var eft = BeaconInfo.FrameTypeForFrame(serviceData);
                if (eft == EddystoneFrameType.TelemetryFrameType)
                {
                    if (!_deviceIDCache.ContainsKey(peripheral.Identifier))
                        _deviceIDCache.Add(peripheral.Identifier, BeaconInfo.TelemetryDataForFrame(serviceData));
                }
                else if (eft == EddystoneFrameType.UIDFrameType || eft == EddystoneFrameType.EIDFrameType)
                {
                    if (peripheral.Identifier != null && _deviceIDCache.ContainsKey(peripheral.Identifier))
                    {
                        var telemetry = _deviceIDCache[peripheral.Identifier];
                        var rssi = RSSI.Int32Value;

                        var beaconInfo = (eft == EddystoneFrameType.UIDFrameType
                            ? BeaconInfo.BeaconInfoForUIDFrameData(serviceData, telemetry, rssi)
                            : BeaconInfo.BeaconInfoForEIDFrameData(serviceData, telemetry, rssi));

                        if (beaconInfo != null)
                        {
                            _deviceIDCache.Remove(peripheral.Identifier);

                            if (_seenEddystoneCache.ContainsKey(new NSString(beaconInfo.BeaconID.Description)))
                            {
                                var timer = _seenEddystoneCache.ObjectForKey(new NSString(beaconInfo.BeaconID.Description + "_onLostTimer")) as NSTimer;
                                timer.Invalidate();
                                timer = NSTimer.CreateScheduledTimer(_onLostTimeout, t =>
                                    {
                                        var cacheKey = beaconInfo.BeaconID.Description;
                                        if (_seenEddystoneCache.ContainsKey(new NSString(cacheKey)))
                                        {
                                            var lostBeaconInfo = _seenEddystoneCache.ObjectForKey(new NSString(cacheKey)) as BeaconInfo;
                                            if (lostBeaconInfo != null)
                                            {
                                                _seenEddystoneCache.Remove(new NSString(beaconInfo.BeaconID.Description));
                                                _seenEddystoneCache.Remove(new NSString(beaconInfo.BeaconID.Description + "_onLostTimer"));

                                                DidLoseBeacon(lostBeaconInfo);
                                            }
                                        }
                                    });

                                DidUpdateBeacon(beaconInfo);
                            }
                            else
                            {
                                DidFindBeacon(beaconInfo);

                                var timer = NSTimer.CreateScheduledTimer(_onLostTimeout, t =>
                                    {
                                        var cacheKey = beaconInfo.BeaconID.Description;
                                        if (_seenEddystoneCache.ContainsKey(new NSString(cacheKey)))
                                        {
                                            var lostBeaconInfo = _seenEddystoneCache.ObjectForKey(new NSString(cacheKey)) as BeaconInfo;
                                            if (lostBeaconInfo != null)
                                            {
                                                _seenEddystoneCache.Remove(new NSString(beaconInfo.BeaconID.Description));
                                                _seenEddystoneCache.Remove(new NSString(beaconInfo.BeaconID.Description + "_onLostTimer"));

                                                DidLoseBeacon(lostBeaconInfo);
                                            }
                                        }
                                    });

                                _seenEddystoneCache.SetValueForKey(beaconInfo, new NSString(beaconInfo.BeaconID.Description));
                                _seenEddystoneCache.SetValueForKey(timer, new NSString(beaconInfo.BeaconID.Description + "_onLostTimer"));
                            }
                        }
                    }
                }
                else if (eft == EddystoneFrameType.URLFrameType)
                {
                    var rssi = RSSI.Int32Value;
                    var url = BeaconInfo.ParseUrlFromFrame(serviceData);

                    var name = peripheral.Name;
                    var services = peripheral.Services;

                    DidObserveURLBeacon(url, rssi);
                }
                else
                {
                    MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", "Unable to find service data; can't process Eddystone");
                }
            }
            else
            {

            }
        }

        #region Eddystone monitoring

        private void StartScanningSynchronized()
        {
            if (_centralManager.State != CBCentralManagerState.PoweredOn)
            {
                _shouldBeScanning = true;
                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", $"CentralManager state is {_centralManager.State}, cannot start scan");
            }
            else
            {
                MvxTrace.TaggedTrace(MvxTraceLevel.Diagnostic, "Beacons", "Starting to scan for Eddystones");

                var peripheralUuids = new List<CBUUID>();
                peripheralUuids.Add(CBUUID.FromString("FEAA")); //eddystone service id

                //peripheralUuids.Add(CBUUID.FromString("0215")); //ibeacon service id, but don't work
                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x02, 0x15 }));

                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x02, 0x01 }));
                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x02, 0x01, 0x1a, 0x1a }));
                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x1a, 0x1a }));
                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x4c, 0x00 }));
                //peripheralUuids.Add(CBUUID.FromBytes(new byte[] { 0x4c, 0x00, 0x02, 0x15 }));

                // 02 01 1a 1a ff 4c 00 02 15 – #Apple's fixed iBeacon advertising prefix
                // стартовать поиск всех устройств BLE и отбирать среди них тех, у кого есть последовательность байт как в предыдущей строке

                //Полезные ссылки:
                //https://github.com/AltBeacon/android-beacon-library/blob/master/src/main/java/org/altbeacon/beacon/BeaconParser.java
                //https://glimwormbeacons.com/learn/what-makes-an-ibeacon-an-ibeacon/
                //http://stackoverflow.com/questions/20387327/using-corebluetooth-with-ibeacons

                _centralManager.ScanForPeripherals(
                    peripheralUuids.ToArray(),
                    new PeripheralScanningOptions(NSDictionary.FromObjectAndKey(NSObject.FromObject(true), CBCentralManager.ScanOptionAllowDuplicatesKey))
                );
            }
        }

        private void DidUpdateBeacon(BeaconInfo beaconInfo)
        {

        }

        private async Task DidFindBeacon(BeaconInfo beaconInfo)
        {
            await BeaconsService.Instance.ActivateEddystoneUid(beaconInfo.BeaconID.ToString());
        }

        private void DidLoseBeacon(BeaconInfo lostBeaconInfo)
        {

        }

        private async Task DidObserveURLBeacon(string url, int rssi)
        {
            await BeaconsService.Instance.ActivateEddystoneUrl(url);
        }

        #endregion

        #endregion
    }
}

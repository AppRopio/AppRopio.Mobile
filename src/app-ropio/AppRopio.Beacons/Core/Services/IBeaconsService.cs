using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Beacons.Responses;

namespace AppRopio.Beacons.Core.Services
{
    public interface IBeaconsService
    {
        Task<List<BeaconModel>> LoadBeaconsByUserLocation(double latitude, double longitude);

        Task ActivateiBeacon(string uuid, int major, int minor);

        Task ActivateEddystoneUrl(string url);

        Task ActivateEddystoneUid(string uid);
    }
}

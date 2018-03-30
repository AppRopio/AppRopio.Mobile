using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Beacons.Responses;

namespace AppRopio.Beacons.API.Services.Fakes
{
    public class BeaconsService : IBeaconsService
    {
        public BeaconsService(IConnectionService connectionService)
        {
            
        }

        public async Task ActivateEddystoneUid(string uid)
        {
            await Task.Delay(300);
        }

        public async Task ActivateEddystoneUrl(string url)
        {
            await Task.Delay(300);
        }

        public async Task ActivateiBeacon(string uuid, int major, int minor)
        {
            await Task.Delay(300);
        }

        public async Task<List<BeaconModel>> LoadBeacons(double latitude, double longitude)
        {
            return new List<BeaconModel>
            {
                new BeaconModel { UUID = "4c2c29fa-c9c7-427f-9cfa-d36da07c4111", Major = 0x0706, Minor = 0x1255 },
                new BeaconModel { UUID = "fb5bf6a4-8d99-479e-afe9-62b75b00ed67", Major = 0x0001, Minor = 0x0018 },
                new BeaconModel { UUID = "fb5bf6a4-8d99-479e-afe9-62b75b00ed67", Major = 0x0001, Minor = 0x0096 }
            };
        }
    }
}

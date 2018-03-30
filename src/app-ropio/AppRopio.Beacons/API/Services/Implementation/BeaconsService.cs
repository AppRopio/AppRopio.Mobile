using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.API.Helpers;
using AppRopio.Base.API.Services;
using AppRopio.Models.Beacons.Responses;
using Newtonsoft.Json;

namespace AppRopio.Beacons.API.Services.Implementation
{
    public class BeaconsService : IBeaconsService
    {
        #region Fields

        private readonly IConnectionService connectionService;

        protected string NEAR_BEACONS_URL = "beacons/near?latitude={0}&longitude={1}";

        protected string IBEACON_ACTIVATED_URL = "beacons/activated_ibeacon";

        protected string EDDYSTONE_URL_ACTIVATED_URL = "beacons/activated_eddystone_url";

        protected string EDDYSTONE_UID_ACTIVATED_URL = "beacons/activated_eddystone_uid";

        #endregion

        #region Constructor

        public BeaconsService(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }

        public async Task<List<BeaconModel>> LoadBeacons(double latitude, double longitude)
        {
            var result = await connectionService.GET(string.Format(NEAR_BEACONS_URL, latitude, longitude));

            if (result.Succeeded)
            {
                var parseResult = await result.Parse<List<BeaconModel>>(Base.API.Models.MediaTypeFormat.Json);
                if (parseResult.Successful)
                    return parseResult.ParsedObject;
            }

            throw new ConnectionException(result);
        }

        public async Task ActivateiBeacon(string uuid, int major, int minor)
        {
            var result = await connectionService.POST(IBEACON_ACTIVATED_URL, new StringContent(JsonConvert.SerializeObject(new { uuid, major, minor })));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }

        public async Task ActivateEddystoneUrl(string url)
        {
            var result = await connectionService.POST(EDDYSTONE_URL_ACTIVATED_URL, new StringContent(JsonConvert.SerializeObject(new { url })));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }

        public async Task ActivateEddystoneUid(string uid)
        {
            var result = await connectionService.POST(EDDYSTONE_UID_ACTIVATED_URL, new StringContent(JsonConvert.SerializeObject(new { uid })));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }

        #endregion
    }
}

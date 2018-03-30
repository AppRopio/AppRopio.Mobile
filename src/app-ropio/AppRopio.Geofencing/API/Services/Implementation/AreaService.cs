using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.API.Helpers;
using System.Net.Http;
using Newtonsoft.Json;
using AppRopio.Models.Geofencing.Responses;

namespace AppRopio.Geofencing.API.Services.Implementation
{
    public class AreaService : IAreaService
    {
        #region Fields

        private readonly IConnectionService connectionService;

        protected string NEAR_AREAS_URL = "geofencing/near?latitude={0}&longitude={1}";

        protected string REGION_ACTIVATED_URL = "geofencing/activated";

        #endregion

        #region Constructor

        public AreaService(IConnectionService connectionService)
        {
            this.connectionService = connectionService;
        }

        #endregion

        #region IAreaService implementation

        public async Task<List<GeofencingModel>> LoadNearAreas(double latitude, double longitude)
        {
            var result = await connectionService.GET(string.Format(NEAR_AREAS_URL, latitude, longitude));

            if (result.Succeeded)
            {
                var parseResult = await result.Parse<List<GeofencingModel>>(Base.API.Models.MediaTypeFormat.Json);
                if (parseResult.Successful)
                    return parseResult.ParsedObject;
            }

            throw new ConnectionException(result);
        }

        public async Task RegionActivated(string id)
        {
            var result = await connectionService.POST(REGION_ACTIVATED_URL, new StringContent(JsonConvert.SerializeObject(new { id })));

            if (result.Succeeded)
                return;

            throw new ConnectionException(result);
        }

        #endregion
    }
}

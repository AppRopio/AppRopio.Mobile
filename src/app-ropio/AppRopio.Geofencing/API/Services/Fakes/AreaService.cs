using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Geofencing.Responses;

namespace AppRopio.Geofencing.API.Services.Fakes
{
    public class AreaService : IAreaService
    {
        private static List<GeofencingModel> _geofencing = new List<GeofencingModel>
        {
            new GeofencingModel { ID = "0", Latitude = 59.907478, Longitude = 30.323961, Radius = 100 } //NOTISSIMUS
        };

        public AreaService(IConnectionService connectionService)
        {
        }

        public async Task<List<GeofencingModel>> LoadNearAreas(double latitude, double longitude)
        {
            return _geofencing;
        }

        public async Task RegionActivated(string id)
        {
            return;
        }
    }
}

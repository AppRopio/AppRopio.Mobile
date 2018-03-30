using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Geofencing.Responses;

namespace AppRopio.Geofencing.API.Services
{
    public interface IAreaService
    {
        Task<List<GeofencingModel>> LoadNearAreas(double latitude, double longitude);

        Task RegionActivated(string id);
    }
}

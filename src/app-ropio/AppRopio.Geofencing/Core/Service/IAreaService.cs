using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppRopio.Models.Geofencing.Responses;

namespace AppRopio.Geofencing.Core.Service
{
    public interface IAreaService
    {
        Task<List<GeofencingModel>> LoadAreasByUserLocation(double latitude, double longitude);
        
        Task ActivateRegionBy(string id);
    }
}

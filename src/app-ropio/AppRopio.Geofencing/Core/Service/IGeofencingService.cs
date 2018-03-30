using System;
using System.Threading.Tasks;
namespace AppRopio.Geofencing.Core.Service
{
    public interface IGeofencingService
    {
        Task<bool> Start();

        Task Stop();
    }
}

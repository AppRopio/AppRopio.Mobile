using System;
using System.Threading.Tasks;
namespace AppRopio.Beacons.Core.Services
{
    public interface IBeaconsScanService
    {
        Task Start();

        Task Stop();
    }
}

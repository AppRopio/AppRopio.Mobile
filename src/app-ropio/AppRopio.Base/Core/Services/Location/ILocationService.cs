using System.Threading.Tasks;
using AppRopio.Models.Base.Responses;

namespace AppRopio.Base.Core.Services.Location
{
    public interface ILocationService
    {
        Coordinates CurrentOrLastLocation { get; }

        Task<Coordinates> GetCurrentLocation();
    }
}

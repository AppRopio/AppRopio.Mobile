using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Map.Responses;
using AppRopio.Models.Base.Responses;

namespace AppRopio.Base.Map.API.Services
{
    public interface IPointsService
    {
        Task<List<Point>> GetPoints(Coordinates position, string searchText, int offset = 0, int count = 10);
    }
}

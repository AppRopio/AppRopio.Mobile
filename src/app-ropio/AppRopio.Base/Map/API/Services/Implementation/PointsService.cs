using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Map.Responses;
using AppRopio.Models.Map.Requests;
using AppRopio.Models.Base.Responses;

namespace AppRopio.Base.Map.API.Services.Implementation
{
    public class PointsService : BaseService, IPointsService
    {
        protected string POINTS = "map/points";

        public async Task<List<Point>> GetPoints(Coordinates position, string searchText, int offset = 0, int count = 10)
        {
            return await Post<List<Point>>(POINTS, ToStringContent(new PointsRequest
            {
                Position = position,
                SearchText = searchText,
                Offset = offset,
                Count = count
            }));
        }
    }
}

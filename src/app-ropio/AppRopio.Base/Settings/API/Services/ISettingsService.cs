using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Settings.Responses;

namespace AppRopio.Base.Settings.API.Services
{
    public interface ISettingsService
    {
        Task<List<RegionGroup>> GetRegions();

		Task<Region> GetRegion(string id);

        Task<List<RegionGroup>> SearchRegions(string query);

        Task<Region> GetCurrentRegion();
    }
}
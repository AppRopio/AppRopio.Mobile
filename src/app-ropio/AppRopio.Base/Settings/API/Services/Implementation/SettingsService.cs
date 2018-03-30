using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Settings.Responses;

namespace AppRopio.Base.Settings.API.Services.Implementation
{
    public class SettingsService : BaseService, ISettingsService
    {
		protected string REGIONS_URL = "regions";
        protected string REGION_URL = "regions/region?id={0}";
        protected string SEARCH_URL = "regions/search?query={0}";
        protected string CURRENT_REGION_URL = "regions/current";

        public async Task<List<RegionGroup>> GetRegions()
		{
            return await Get<List<RegionGroup>>(REGIONS_URL);
		}

		public async Task<Region> GetRegion(string id)
		{
            var url = string.Format(REGION_URL, id);
            return await Get<Region>(url);
		}

		public async Task<List<RegionGroup>> SearchRegions(string query)
		{
            var url = string.Format(SEARCH_URL, query);
            return await Get<List<RegionGroup>>(url);
		}

		public async Task<Region> GetCurrentRegion()
		{
            return await Get<Region>(CURRENT_REGION_URL);
		}
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.API.Services.Implementation
{
    public class FiltersService : BaseService, IFiltersService
    {
        protected string SORT_TYPES_URL = "sorttypes";
        protected string FILTERS_URL = "filters";

        public async Task<List<Filter>> LoadFilters(string categoryId)
        {
            return await Get<List<Filter>>(
                string.IsNullOrEmpty(categoryId) ? FILTERS_URL : $"{FILTERS_URL}?categoryId={categoryId}"
            );
        }

        public async Task<List<SortType>> LoadSortTypes(string categoryId)
        {
            return await Get<List<SortType>>(
                string.IsNullOrEmpty(categoryId) ? SORT_TYPES_URL : $"{SORT_TYPES_URL}?categoryId={categoryId}"
            );
        }
    }
}

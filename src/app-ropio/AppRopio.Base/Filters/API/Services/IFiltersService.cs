using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.API
{
    public interface IFiltersService
    {
        Task<List<Filter>> LoadFilters(string categoryId);

        Task<List<SortType>> LoadSortTypes(string categoryId);
    }
}

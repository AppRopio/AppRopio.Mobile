using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Services
{
    public interface IFiltersVmService
    {
        Task<ObservableCollection<IFiltersItemVM>> LoadFiltersFor(string categoryId, List<ApplyedFilter> applyedFilters);

        void ChangeFiltersTo(string categoryId, List<ApplyedFilter> applyedFilters);
    }
}

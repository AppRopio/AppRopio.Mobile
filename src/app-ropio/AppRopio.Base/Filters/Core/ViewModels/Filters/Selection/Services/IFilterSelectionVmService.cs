using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels.Selection.Services;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Selection.Services
{
    public interface IFilterSelectionVmService : IBaseSelectionVmService<FilterValue, ApplyedFilterValue>
    {
        void ChangeSelectedFiltersTo(string filterId, List<ApplyedFilterValue> selectedValues);
    }
}

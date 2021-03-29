using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.Core.Models.Bundle;

namespace AppRopio.Base.Filters.Core.Services
{
    public interface IFiltersNavigationVmService : IBaseVmNavigationService
    {
        void NavigateToFilters(FiltersBundle bundle);

        void NavigateToSelection(SelectionBundle bundle);

        void NavigateToSort(SortBundle bundle);
    }
}

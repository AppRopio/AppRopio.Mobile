using System;
using AppRopio.Base.Filters.Core.Models.Bundle;

namespace AppRopio.Base.Filters.Core.Services
{
    public interface IFiltersNavigationVmService
    {
        void NavigateToFilters(FiltersBundle bundle);

        void NavigateToSelection(SelectionBundle bundle);

        void NavigateToSort(SortBundle bundle);
    }
}

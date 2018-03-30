using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Sort;

namespace AppRopio.Base.Filters.Core.Services.Implementation
{
    public class FiltersNavigationVmService : BaseVmNavigationService, IFiltersNavigationVmService
    {
        public void NavigateToFilters(FiltersBundle bundle)
        {
            NavigateTo<IFiltersViewModel>(bundle);
        }

        public void NavigateToSelection(SelectionBundle bundle)
        {
            NavigateTo<IFilterSelectionViewModel>(bundle);
        }

        public void NavigateToSort(SortBundle bundle)
        {
            NavigateTo<ISortViewModel>(bundle);
        }
    }
}

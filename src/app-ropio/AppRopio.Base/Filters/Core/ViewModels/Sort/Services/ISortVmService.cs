using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Items;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Filters.Core.ViewModels.Sort.Services
{
    public interface ISortVmService
    {
        Task<ObservableCollection<ISortItemVM>> LoadSortTypesInCategory(string categoryId, string selectedSortId);

        void ChangeSortTypeTo(string categoryId, SortType sort);
    }
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection
{
    public interface IBaseCollectionFiVm : IFiltersItemVM
    {
        ICommand SelectionChangedCommand { get; }

        ObservableCollection<CollectionItemVM> Items { get; }

        List<FilterValue> Values { get; }
    }
}

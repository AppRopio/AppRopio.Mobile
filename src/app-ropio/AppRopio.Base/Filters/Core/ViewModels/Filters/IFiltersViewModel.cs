using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters
{
    public interface IFiltersViewModel : IBaseViewModel
    {
        ICommand SelectionChangedCommand { get; }

        ICommand ApplyCommand { get; }

        ICommand ClearCommand { get; }

        ObservableCollection<IFiltersItemVM> Items { get; }
    }
}

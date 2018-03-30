using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Items;

namespace AppRopio.Base.Filters.Core.ViewModels.Sort
{
    public interface ISortViewModel : IBaseViewModel
    {
        ICommand SelectionChangedCommand { get; }

        ICommand CancelCommand { get; }

        ObservableCollection<ISortItemVM> Items { get; }
    }
}

using System;
using System.Collections.ObjectModel;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection.Items;
using System.Windows.Input;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection
{
    public interface IMultiSelectionFiVm : IBaseSelectionFiVm
    {
        ICommand SelectionChangedCommand { get; }

        ObservableCollection<MultiCollectionItemVM> Items { get; }
    }
}

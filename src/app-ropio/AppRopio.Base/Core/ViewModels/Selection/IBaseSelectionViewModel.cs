using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Core.ViewModels.Selection.Items;

namespace AppRopio.Base.Core.ViewModels.Selection
{
    public interface IBaseSelectionViewModel : ISearchViewModel
    {
        string Name { get; }

        ICommand SelectionChangedCommand { get; }

        ICommand ApplyCommand { get; }

        ICommand ClearCommand { get; }

        ObservableCollection<ISelectionItemVM> Items { get; }

        string ApplyTitle { get; }
    }
}

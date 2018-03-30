using System;
using System.Collections.Generic;
using System.Windows.Input;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker.Items;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker
{
    public interface IPickerFiVm : IFiltersItemVM, ISelectableFilterItemVM
    {
        bool Selected { get; }

        ICommand SelectionChangedCommand { get; }

        List<PickerCollectionItemVM> Items { get; }

        string ValueName { get; }

        int SelectedItemIndex { get; }

        PickerCollectionItemVM SelectedItem { get; }
    }
}

using System.Collections.Generic;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker.Items;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker
{
    public interface IPickerPciVm : IProductDetailsItemVM, ISelectableProductCardItemVM
    {
        bool Selected { get; }

        ICommand SelectionChangedCommand { get; }

        List<PickerCollectionItemVM> Items { get; }

        string ValueName { get; }

        int SelectedItemIndex { get; }

        PickerCollectionItemVM SelectedItem { get; }
    }
}

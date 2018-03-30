using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection.Items;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection
{
    public interface IMultiSelectionPciVm : IBaseSelectionPciVm
    {
        ICommand SelectionChangedCommand { get; }

        ObservableCollection<MultiCollectionItemVM> Items { get; }
    }
}

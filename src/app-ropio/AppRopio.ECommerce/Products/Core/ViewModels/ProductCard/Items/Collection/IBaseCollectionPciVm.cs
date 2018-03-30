using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection
{
    public interface IBaseCollectionPciVm<TItemVM, TValue> : IBaseCollectionPciVm
    	where TItemVM : class, IMvxViewModel
		where TValue : class
    {
		ObservableCollection<TItemVM> Items { get; }

		List<TValue> Values { get; }
    }
    
    public interface IBaseCollectionPciVm : IProductDetailsItemVM
    {
        ICommand SelectionChangedCommand { get; }
    }
}

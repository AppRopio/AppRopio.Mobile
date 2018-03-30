using System;
using System.Windows.Input;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax
{
    public interface IBaseMinMaxPciVm : IProductDetailsItemVM
    {
        ICommand MinValueChangedCommand { get; }

        ICommand MaxValueChangedCommand { get; }
    }

    public interface IBaseMinMaxPciVm<T> : IBaseMinMaxPciVm
        where T : IComparable, IComparable<T>, IEquatable<T>
    {
        T Min { get; }

        T Max { get; }
    }
}

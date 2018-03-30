using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Transition
{
    public interface ITransitionPciVm : IProductDetailsItemVM, ISelectableProductCardItemVM
    {
        string Value { get; }
    }
}

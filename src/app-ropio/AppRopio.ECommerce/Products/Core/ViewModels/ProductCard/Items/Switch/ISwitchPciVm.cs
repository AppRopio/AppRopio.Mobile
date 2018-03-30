using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Switch
{
    public interface ISwitchPciVm : IProductDetailsItemVM, ISelectableProductCardItemVM
    {
        bool Enabled { get; }
    }
}

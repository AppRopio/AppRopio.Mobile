using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items
{
    public interface IProductBadgeItemVM : IMvxViewModel
    {
        string Name { get; }

        string Color { get; }
    }
}

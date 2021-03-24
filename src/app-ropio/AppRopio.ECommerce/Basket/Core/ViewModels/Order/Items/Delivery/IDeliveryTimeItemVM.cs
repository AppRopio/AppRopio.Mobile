using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public interface IDeliveryTimeItemVM : IMvxViewModel
    {
        string Id { get; }

        string Name { get; }
    }
}

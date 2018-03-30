using System.Collections.Generic;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public interface IDeliveryDayItemVM : IMvxViewModel
    {
        string Id { get; }

        string Name { get; }

        List<IDeliveryTimeItemVM> Times { get; }
    }
}

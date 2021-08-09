using System.Collections.Generic;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public interface IOrderFieldsGroupVM : IMvxViewModel
    {
        string Id { get; }

        string Name { get; }

        List<IOrderFieldItemVM> Items { get; }
    }
}

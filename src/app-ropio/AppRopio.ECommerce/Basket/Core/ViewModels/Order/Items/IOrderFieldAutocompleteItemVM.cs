using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public interface IOrderFieldAutocompleteItemVM : IMvxViewModel
    {
        string Value { get; }
    }
}

using System;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public interface IOrderFieldAutocompleteItemVM : IMvxViewModel
    {
        string Value { get; }
    }
}

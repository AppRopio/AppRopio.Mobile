using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator
{
    public interface IBasketCartIndicatorViewModel : IMvxViewModel
    {
        IMvxCommand BasketCommand { get; }

        string Quantity { get; }
    }
}

using System;
using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks
{
    public interface IThanksViewModel : IMvxViewModel
    {
        ICommand GoToCatalogCommand { get; }

        ICommand CloseCommand { get; }

        string OrderId { get; }
    }
}
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment
{
    public interface IPaymentViewModel : IBaseViewModel
    {
        ICommand SelectionChangedCommand { get; }

        ICommand CancelCommand { get; }

        ObservableCollection<IPaymentItemVM> Items { get; }
    }
}

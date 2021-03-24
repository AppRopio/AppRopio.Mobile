using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full
{
    public interface IFullOrderViewModel : IBaseViewModel, IOrderViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        IUserViewModel UserViewModel { get; }

        IDeliveryViewModel DeliveryViewModel { get; }

        MvxObservableCollection<IMvxViewModel> Items { get; }
    }
}

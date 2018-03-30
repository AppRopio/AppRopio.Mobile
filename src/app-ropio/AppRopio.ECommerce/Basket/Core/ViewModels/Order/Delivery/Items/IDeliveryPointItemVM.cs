using MvvmCross.Core.ViewModels;
using System.Windows.Input;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items
{
    public interface IDeliveryPointItemVM : IMvxViewModel, IHasCoordinates
    {
        string Id { get; }

        string Name { get; }

        string Address { get; }

        string WorkTime { get; }

        string Distance { get; }

        string Phone { get; }

        string AdditionalInfo { get; }

        bool IsSelected { get; set; }

        ICommand CallCommand { get; }

        ICommand AdditionalInfoCommand { get; }

        ICommand MapCommand { get; }

        ICommand RouteCommand { get; }
    }
}

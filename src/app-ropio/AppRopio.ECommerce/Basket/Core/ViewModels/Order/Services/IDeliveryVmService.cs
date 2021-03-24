using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.Models.Base.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services
{
    public interface IDeliveryVmService
    {
        Task<MvxObservableCollection<IDeliveryTypeItemVM>> LoadDeliveryTypes();

        Task<MvxObservableCollection<IDeliveryPointItemVM>> LoadDeliveryPoints(string deliveryId, string searchText);

        Task<MvxObservableCollection<IOrderFieldItemVM>> LoadDeliveryAddressFields(string deliveryId, Coordinates coordinates = null);

        Task<bool> ValidateAndSaveDeliveryAddressFields(string deliveryId, MvxObservableCollection<IOrderFieldItemVM> fields);

        Task<bool> ValidateAndSaveDeliveryPoint(string deliveryId, string deliveryPointId);

        Task<decimal?> LoadDeliveryPrice(string deliveryId);

        Task<MvxObservableCollection<IDeliveryDayItemVM>> LoadDeliveryTime(string deliveryId);

        Task<bool> ConfirmDeliveryTime(string deliveryTimeId);

        Task<bool> ValidateAndSaveDelivery(string id);
    }
}

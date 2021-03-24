using System.Collections.ObjectModel;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public interface IDeliveryOnAddressVM : IBaseDeliveryVM
    {
        /// <summary>
        /// Набор полей для заполнения адреса доставки, даты и пр.
        /// </summary>
        MvxObservableCollection<IOrderFieldItemVM> AddressFieldsItems { get; }
    }
}

using System;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public interface IDeliveryPointOnMapVM : IBaseViewModel
    {
        IDeliveryPointItemVM Item { get; }

        /// <summary>
        /// Закрытие VM (для модальных экранов)
        /// </summary>
        ICommand CloseCommand { get; }
    }
}

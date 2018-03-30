using System.Windows.Input;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public interface IBaseDeliveryVM : IOrderViewModel
    {
        /// <summary>
        /// Идентификатор способа доставки
        /// </summary>
        string DeliveryId { get; }

        /// <summary>
        /// Стоимость доставки
        /// </summary>
        decimal? DeliveryPrice { get; }

        /// <summary>
        /// Закрытие VM (для модальных экранов)
        /// </summary>
        ICommand CloseCommand { get; }
    }
}

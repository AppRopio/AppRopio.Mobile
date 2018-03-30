using AppRopio.Base.Core.ViewModels;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order
{
    public interface IOrderViewModel : IBaseViewModel
    {
        /// <summary>
        /// Итоговая сумма корзины
        /// </summary>
        decimal BasketAmount { get; }

        /// <summary>
        /// Итоговая сумма заказа (с учетом доставки)
        /// </summary>
        decimal Amount { get; }

        /// <summary>
        /// Доступность перехода к следующим этапам
        /// </summary>
        bool CanGoNext { get; }

		/// <summary>
		/// Переход к следующим этапам оформления заказа
		/// </summary>
		IMvxCommand NextCommand { get; }
    }
}

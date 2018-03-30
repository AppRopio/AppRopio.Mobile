using System;
namespace AppRopio.Analytics.GoogleAnalytics.Core.Services
{
    public interface IGoogleAnalytics
    {
        /// <summary>
        /// Отслеживание открытия экрана
        /// </summary>
        /// <param name="screenName">Название экрана</param>
        void TrackScreen(string screenName);

        /// <summary>
        /// Отслеживание события
        /// </summary>
        /// <param name="category">Категория события, например, ui_action.</param>
        /// <param name="action">Действие, связанное с событием, например, button_press.</param>
        /// <param name="label">Ярлык события, например, play.</param>
        void TrackEvent(string category, string action, string label = null);

        /// <summary>
        /// Отслеживание ошибок и исключений
        /// </summary>
        /// <param name="msg">Сообщение.</param>
        /// <param name="isFatal">Если <c>true</c> то ошибка фатальная.</param>
        void TrackException(string msg, bool isFatal);

        /// <summary>
        /// Отслеживание покупок
        /// </summary>
        /// <param name="fullPrice">Стоимость корзины.</param>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <param name="currency">Валюта.</param>
        void TrackECommerce(decimal fullPrice, string orderId, string currency);
    }
}

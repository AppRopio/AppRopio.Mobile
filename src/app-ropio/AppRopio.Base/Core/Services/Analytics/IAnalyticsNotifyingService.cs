using System.Collections.Generic;
using AppRopio.Base.Core.Models.Analytics;

namespace AppRopio.Base.Core.Services.Analytics
{
    public interface IAnalyticsNotifyingService
    {
        /// <summary>
        /// Уведомить сервисы аналитики об изменении состояния приложения
        /// </summary>
        /// <param name="state">Текущее состояние приложения.</param>
        /// <param name="data">Дополнительные данные</param>
        void NotifyApp(AppState state, Dictionary<string, string> data = null);

        /// <summary>
        /// Уведомить сервисы аналитики, что был открыт экран
        /// </summary>
        /// <param name="screenName">Название экрана</param>
        /// <param name="data">Дополнительные данные</param>
        void NotifyScreen(string screenName, ScreenState state, Dictionary<string, string> data = null);

        /// <summary>
        /// Уведомить сервисы аналитики, что произошло некое событие
        /// </summary>
        /// <param name="category">Категория события, например, basket.</param>
        /// <param name="action">Действие, связанное с событием, например, button_press.</param>
        /// <param name="label">Ярлык события, например, add to basket.</param>
        /// <param name="model">Модель, связанная с событием, например, product</param>
        /// <param name="data">Дополнительные данные</param>
        void NotifyEventIsHandled(string category, string action, string label = null, object model = null, Dictionary<string, string> data = null);

        /// <summary>
        /// Уведомить сервисы аналитики, что был оформлен заказ
        /// </summary>
        /// <param name="fullPrice">Итоговая стоимость корзины с учетом скидок.</param>
        /// <param name="quantity">Количество товаров.</param>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <param name="currency">Валюта заказа.</param>
        /// <param name="data">Дополнительные данные</param>
        void NotifyOrderPurchased(decimal fullPrice, float quantity, string orderId, string currency, Dictionary<string, string> data = null);

        /// <summary>
        /// Уведомить сервисы аналитики об исключении или ошибке
        /// </summary>
        /// <param name="message">Сообщение ошибки.</param>
        /// <param name="stackTrace">Stack trace.</param>
        /// <param name="isFatal">Если <c>true</c> значит ошибка критическая.</param>
        void NotifyExceptionHandled(string message, string stackTrace, bool isFatal, Dictionary<string, string> data = null);
    }
}

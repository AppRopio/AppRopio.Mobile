using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Basket.Responses.Order;

namespace AppRopio.ECommerce.Basket.API.Services
{
    public interface IOrderService
    {
        /// <summary>
        /// Возвращает сгруппированный список полей для заполнения информации о пользователе, получателе и пр.
        /// </summary>
        Task<List<OrderFieldsGroup>> GetUserFieldsGroups();

        /// <summary>
        /// Сохраняет информацию о пользователе и возвращает результат валиадции.
        /// </summary>
        /// <param name="userFieldsIdValues">Набор пар: Id поля — Значение поля</param>
        Task<FieldsValidation> ConfirmUser(Dictionary<string, string> userFieldsIdValues);

        /// <summary>
        /// Возвращает настройки, определяющие необходимость загрузки и выбора способа оплаты
        /// </summary>
        Task<PaymentNecessary> GetPaymentNecessary(string deliveryId);

        /// <summary>
        /// Возвращает список способов оплаты, относительно выбранного типа доставки.
        /// </summary>
        Task<List<Payment>> GetPayments(string deliveryId);

        /// <summary>
        /// Сохраняет выбранный способ оплаты
        /// </summary>
        Task ConfirmPayment(string paymentId);

        /// <summary>
        /// Получение информации о подтвержденном заказе (для аналитики)
        /// </summary>
        Task<ConfirmedOrderInfo> ConfirmedOrderInfo(string orderId);

        Task<Order> CreateOrder();

        Task ConfirmOrder(string orderId, bool isPaid);

        /// <summary>
        /// Возвращает список значений для автозаполнения поля ввода
        /// </summary>
        /// <param name="dependentFieldsValues">Набор пар: Id поля — Значение поля</param>
        Task<List<OrderFieldAutocompleteValue>> GetAutocompleteValues(string fieldId, string value, Dictionary<string, string> dependentFieldsValues);
    }
}

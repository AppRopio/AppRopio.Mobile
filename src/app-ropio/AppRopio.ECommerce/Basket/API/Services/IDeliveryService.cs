using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Basket.Responses.Order;
using AppRopio.Models.Base.Responses;

namespace AppRopio.ECommerce.Basket.API.Services
{
    public interface IDeliveryService
    {
        /// <summary>
        /// Возвращает список доступных способов доставки.
        /// </summary>
        Task<List<Delivery>> GetDeliveries();

        /// <summary>
        /// Возвращает список точек самовывоза, относительно выбранного типа доставки.
        /// </summary>
        Task<List<DeliveryPoint>> GetDeliveryPoints(string deliveryId, string searchText);

        /// <summary>
        /// Возвращает список полей для заполнения информации об адресе доставки, относительно выбранного типа доставки и текущей геопозиции
        /// </summary>
        Task<List<OrderField>> GetDeliveryAddressFields(string deliveryId, Coordinates coordinates = null);

        /// <summary>
        /// Сохраняет информацию о выбранной точке самовывоза, в случае неудачи, возвращает исключение.
        /// </summary>
        Task ConfirmDeliveryPoint(string deliveryId, string deliveryPointId);

        /// <summary>
        /// Сохраняет информацию об адресе доставки и возвращает результат валиадции.
        /// </summary>
        /// <param name="addressFieldsIdValues">Набор пар: Id поля — Значение поля</param>
        Task<FieldsValidation> ConfirmDeliveryAddress(string deliveryId, Dictionary<string, string> addressFieldsIdValues);

        /// <summary>
        /// Возвращает список доступных дат для доставки в зависимости от способа.
        /// </summary>
        Task<List<DeliveryDay>> GetDeliveryTime(string deliveryId);

        Task ConfirmDeliveryTime(string deliveryTimeId);

        /// <summary>
        /// Возвращает стоимость доставки в зависимости от способа.
        /// </summary>
        Task<decimal?> GetDeliveryPrice(string deliveryId);

        /// <summary>
        /// Сохраняет информацию о выбранном способе доставки, когда не требуется для выбранного способа получить от пользователя дополнительные данные
        /// </summary>
        /// <returns>The delivery.</returns>
        /// <param name="id">Identifier.</param>
        Task ConfirmDelivery(string id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Basket.Responses.Order;
using AppRopio.Models.Basket.Requests;
using System.Globalization;
using AppRopio.Models.Base.Responses;

namespace AppRopio.ECommerce.Basket.API.Services.Implementation
{
    public class DeliveryService : BaseService, IDeliveryService
    {
        protected string DELIVERIES = "order/deliveries";
        protected string DELIVERY_POINTS = "order/deliveryPoints";
        protected string DELIVERY_ADDRESS_FIELDS = "order/deliveryAddressFields";
        protected string CONFIRM_DELIVERY = "order/confirmDelivery";
        protected string CONFIRM_DELIVERY_TIME = "order/confirmDeliveryTime";
        protected string CONFIRM_DELIVERY_POINT = "order/confirmDeliveryPoint";
        protected string CONFIRM_DELIVERY_ADDRESS = "order/confirmDeliveryAddress";
        protected string DELIVERY_PRICE = "order/deliveryPrice";
        protected string DELIVERY_TIME = "order/deliveryTime";
        
        public async Task<FieldsValidation> ConfirmDeliveryAddress(string deliveryId, Dictionary<string, string> addressFieldsIdValues)
        {
            return await Post<FieldsValidation>(CONFIRM_DELIVERY_ADDRESS, ToStringContent(new ConfirmDeliveryAddressRequest { DeliveryId = deliveryId, FieldsValues = addressFieldsIdValues }));
        }

        public Task ConfirmDeliveryPoint(string deliveryId, string deliveryPointId)
        {
            return Post(CONFIRM_DELIVERY_POINT, ToStringContent(new ConfirmDeliveryPointRequest { DeliveryId = deliveryId, PointId = deliveryPointId }));
        }

        public async Task<List<Delivery>> GetDeliveries()
        {
            return await Get<List<Delivery>>(DELIVERIES);
        }

        public async Task<List<OrderField>> GetDeliveryAddressFields(string deliveryId, Coordinates coordinates = null)
        {
            return await Post<List<OrderField>>(DELIVERY_ADDRESS_FIELDS, ToStringContent(new DeliveryAddressRequest { DeliveryId = deliveryId, Coordinates = coordinates }));
        }

        public async Task<List<DeliveryPoint>> GetDeliveryPoints(string deliveryId, string searchText)
        {
            return await Post<List<DeliveryPoint>>(DELIVERY_POINTS, ToStringContent(new DeliveryPointRequest { Id = deliveryId, SearchText = searchText }));
        }

        public async Task<decimal?> GetDeliveryPrice(string deliveryId)
        {
            return await Post(DELIVERY_PRICE, ToStringContent(new DeliveryPriceRequest { Id = deliveryId }), (string arg) =>
            {
                decimal? val = null;

                return string.IsNullOrEmpty(arg) ? val : Convert.ToDecimal(arg, NumberFormatInfo.InvariantInfo);
            });
        }

        public async Task<List<DeliveryDay>> GetDeliveryTime(string deliveryId)
        {
            return await Get<List<DeliveryDay>>($"{DELIVERY_TIME}?id={deliveryId}");
        }

        public Task ConfirmDeliveryTime(string deliveryTimeId)
        {
            return Post(CONFIRM_DELIVERY_TIME, ToStringContent(new ConfirmDeliveryTimeRequest { Id = deliveryTimeId }));
        }

        public Task ConfirmDelivery(string id)
        {
            return Post(CONFIRM_DELIVERY, ToStringContent(new ConfirmDeliveryRequest { Id = id }));
        }
    }
}

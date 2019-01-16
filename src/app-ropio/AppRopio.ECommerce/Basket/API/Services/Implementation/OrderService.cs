using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Basket.Responses.Order;
using AppRopio.Models.Basket.Requests;

namespace AppRopio.ECommerce.Basket.API.Services.Implementation
{
    public class OrderService : BaseService, IOrderService
    {
        protected string USER_FIELDS = "order/userFields";
        protected string CONFIRM_USER = "order/confirmUser";
        protected string PAYMENT_NECESSARY = "order/paymentNecessary";
        protected string PAYMENTS = "order/payments";
        protected string CONFIRM_PAYMENTS = "order/confirmPayment";
        protected string CREATE_ORDER = "order/create";
        protected string CONFIRM_ORDER = "order/confirm";
        protected string CONFIRMED_ORDER_INFO = "order/confirmedInfo";
        protected string AUTOCOMPLETE = "order/autocompleteValues";

        public Task ConfirmOrder(string orderId, bool isPaid, string transactionId)
        {
            return Post(CONFIRM_ORDER, ToStringContent(new { id = orderId, isPaid = isPaid, transactionId = transactionId }));
        }

        public Task ConfirmPayment(string paymentId)
        {
            return Post(CONFIRM_PAYMENTS, ToStringContent(new { id = paymentId }));
        }

        public async Task<FieldsValidation> ConfirmUser(Dictionary<string, string> userFieldsIdValues)
        {
            return await Post<FieldsValidation>(CONFIRM_USER, ToStringContent(new ConfirmUserRequest { FieldsValues = userFieldsIdValues }));
        }

        public async Task<Order> CreateOrder()
        {
            return await Get<Order>(CREATE_ORDER);
        }

        public async Task<PaymentNecessary> GetPaymentNecessary(string deliveryId)
        {
            return await Get<PaymentNecessary>($"{PAYMENT_NECESSARY}{(string.IsNullOrEmpty(deliveryId) ? string.Empty : $"?deliveryId={deliveryId}")}");
        }

        public async Task<List<Payment>> GetPayments(string deliveryId)
        {
            return await Get<List<Payment>>($"{PAYMENTS}{(string.IsNullOrEmpty(deliveryId) ? string.Empty : $"?id={deliveryId}")}");
        }

        public async Task<List<OrderFieldsGroup>> GetUserFieldsGroups()
        {
            return await Get<List<OrderFieldsGroup>>(USER_FIELDS);
        }

        public async Task<ConfirmedOrderInfo> ConfirmedOrderInfo(string orderId)
        {
            return await Get<ConfirmedOrderInfo>($"{CONFIRMED_ORDER_INFO}?id={orderId}");
        }

        public async Task<List<OrderFieldAutocompleteValue>> GetAutocompleteValues(string fieldId, string value, Dictionary<string, string> dependentFieldsValues)
        {
            return await Post<List<OrderFieldAutocompleteValue>>(AUTOCOMPLETE, ToStringContent(new OrderFieldAutocompleteRequest { FieldId = fieldId, Value = value, DependentFieldsValues = dependentFieldsValues }));
        }
    }
}
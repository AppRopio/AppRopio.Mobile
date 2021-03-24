using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross;
using OrderModel = AppRopio.Models.Basket.Responses.Order.Order;
using PaymentModel = AppRopio.Models.Basket.Responses.Order.Payment;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services.Implementation
{
    public class OrderVmService : BaseVmService, IOrderVmService
    {
        #region Services

        private IOrderService _apiService;
        public IOrderService ApiService => _apiService ?? (_apiService = Mvx.Resolve<IOrderService>());

        #endregion

        #region Protected

        protected virtual IPaymentItemVM SetupPaymentItem(PaymentModel model)
        {
            return new PaymentItemVM(model);
        }

        protected virtual IOrderFieldAutocompleteItemVM SetupAutocompleteItem(OrderFieldAutocompleteValue model)
        {
            return new OrderFieldAutocompleteItemVM(model);
        }

        #endregion

        #region IOrderVmService implementation

        public async Task<bool> IsPaymentNecessary(string delivaryId)
        {
            var result = false;

            try
            {
                var paymentNecessary = await ApiService.GetPaymentNecessary(delivaryId);
                result = paymentNecessary.IsNecessary;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }

        public async Task<ObservableCollection<IPaymentItemVM>> LoadPayments(string deliveryId)
        {
            ObservableCollection<IPaymentItemVM> source = null;

            try
            {
                var payments = await ApiService.GetPayments(deliveryId);
                source = new ObservableCollection<IPaymentItemVM>(payments.Select(SetupPaymentItem));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return source;
        }

        public async Task<bool> ConfirmPayment(string paymentId)
        {
            var result = false;

            try
            {
                await ApiService.ConfirmPayment(paymentId);
                result = true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }

        public async Task<OrderModel> CreateOrder()
        {
            OrderModel order = null;

            try
            {
                order = await ApiService.CreateOrder();
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return order;
        }

        public async Task<bool> ConfirmOrder(string orderId, bool isPaid)
        {
            try
            {
                await ApiService.ConfirmOrder(orderId, isPaid);
                return true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public async Task<ConfirmedOrderInfo> OrderInfo(string orderId)
        {
            try
            {
                return await ApiService.ConfirmedOrderInfo(orderId);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return new ConfirmedOrderInfo();
        }

        public async Task<ObservableCollection<IOrderFieldAutocompleteItemVM>> LoadAutocompleteValues(string fieldId, string value, Dictionary<string, string> dependentFieldsValues)
        {
            ObservableCollection<IOrderFieldAutocompleteItemVM> source = null;

            try
            {
                var autocompletes = await ApiService.GetAutocompleteValues(fieldId, value, dependentFieldsValues);
                source = new ObservableCollection<IOrderFieldAutocompleteItemVM>(autocompletes.Select(SetupAutocompleteItem));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return source;
        }

        #endregion
    }
}

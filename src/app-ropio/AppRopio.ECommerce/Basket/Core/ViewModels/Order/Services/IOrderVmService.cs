using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items;
using OrderModel = AppRopio.Models.Basket.Responses.Order.Order;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using System.Collections.Generic;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services
{
    public interface IOrderVmService
    {
        Task<bool> IsPaymentNecessary(string deliveryId);

        Task<ObservableCollection<IPaymentItemVM>> LoadPayments(string deliveryId);

        Task<bool> ConfirmPayment(string paymentId);

        Task<OrderModel> CreateOrder();

        Task<bool> ConfirmOrder(string orderId, bool isPaid);

        Task<ConfirmedOrderInfo> OrderInfo(string orderId);

        Task<ObservableCollection<IOrderFieldAutocompleteItemVM>> LoadAutocompleteValues(string fieldId, string value, Dictionary<string, string> dependentFieldsValued);
    }
}

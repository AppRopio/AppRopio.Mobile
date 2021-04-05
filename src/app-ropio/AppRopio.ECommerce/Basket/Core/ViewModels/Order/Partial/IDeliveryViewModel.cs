using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.Payments.Core.Messages;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using PaymentModel = AppRopio.Models.Basket.Responses.Order.Payment;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial
{
    public interface IDeliveryViewModel : IBaseViewModel, IOrderViewModel
    {
        /// <summary>
        /// Выбор способа доставки с последующим переходом на нужный экран
        /// </summary>
        IMvxCommand SelectionChangedCommand { get; }

        /// <summary>
        /// Выбор времени доставки
        /// </summary>
        ICommand ApplyDeliveryTimeCommand { get; }

        /// <summary>
        /// Список способов доставки
        /// </summary>
        MvxObservableCollection<IDeliveryTypeItemVM> Items { get; }

        /// <summary>
        /// Список времени доставки
        /// </summary>
        MvxObservableCollection<IDeliveryDayItemVM> DaysItems { get; }

        MvxObservableCollection<IDeliveryTimeItemVM> TimeItems { get; }

        /// <summary>
        /// Показывать контрол для выбора времени доставки
        /// </summary>
        bool IsShowDeliveryTimePicker { get; }

        bool DeliveryTimeLoading { get; }

        IDeliveryDayItemVM SelectedDeliveryDay { get; }

        IDeliveryTimeItemVM SelectedDeliveryTime { get; }

        string SelectedDeliveryTimeValue { get; }

        /// <summary>
        /// Стоимость доставки
        /// </summary>
        decimal? DeliveryPrice { get; }

        Task LoadContent();

        Task OnDeliveryChanged(IDeliveryTypeItemVM deliveryItem);

        Task<bool> ValidateDelivery(IEnumerable<IDeliveryTypeItemVM> deliveries);

        void OrderProcessingChanged(OrderProcessingMessage msg);

        void OnOrderPaidMessageRecieved(OrderPaidMessage msg);

        void OnPaymentSelectedMessage(PaymentSelectedMessage msg);

        Task CreateOrder(PaymentModel payment = null);
    }
}

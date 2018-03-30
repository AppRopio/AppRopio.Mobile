using System;
using PaymentModel = AppRopio.Models.Basket.Responses.Order.Payment;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items
{
    public interface IPaymentItemVM
    {
        PaymentModel Payment { get; }

        string Title { get; }

        bool IsSelected { get; set; }
    }
}

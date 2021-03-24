using AppRopio.Models.Basket.Responses.Enums;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery
{
    public interface IDeliveryTypeItemVM : IMvxViewModel
    {
        string Id { get; }

        string Name { get; }

        DeliveryType Type { get; }

        decimal? Price { get; }

        bool IsRequiredDataEntry { get; }

        bool IsDeliveryTimeRequired { get; }

        bool IsSelected { get; set; }

        bool NotAvailable { get; }

        string Message { get; }
    }
}

using System;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Platform.Converters;

namespace AppRopio.ECommerce.HistoryOrders.Droid.ValueConverters
{
    public class PaymentStatusTextValueConverter : MvxValueConverter<PaymentStatus, string>
    {
        protected override string Convert(PaymentStatus value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case PaymentStatus.Paid:
                    return "Оплачен";
                case PaymentStatus.NotPaid:
                    return "Не оплачен";
                default:
                    return string.Empty;
            }
        }
    }
}

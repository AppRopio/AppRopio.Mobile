using System;
using System.Globalization;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Platform.Converters;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Converters
{
    public class PaymentStatusToStringConverter : MvxValueConverter<PaymentStatus, string>
    {
        protected override string Convert(PaymentStatus value, Type targetType, object parameter, CultureInfo culture)
		{
            switch (value)
            {
                case PaymentStatus.Paid:
                    return "Оплачен";

                case PaymentStatus.NotPaid:
                    return "Неоплачен";
                default:
                    return null;
            }
		}
    }
}
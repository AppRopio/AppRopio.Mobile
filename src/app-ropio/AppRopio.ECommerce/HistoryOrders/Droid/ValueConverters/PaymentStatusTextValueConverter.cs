using System;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Converters;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.Droid.ValueConverters
{
    public class PaymentStatusTextValueConverter : MvxValueConverter<PaymentStatus, string>
    {
        protected override string Convert(PaymentStatus value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case PaymentStatus.Paid:
                    return Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "Status_Paid");
                case PaymentStatus.NotPaid:
                    return Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "Status_NotPaid");
                default:
                    return string.Empty;
            }
        }
    }
}

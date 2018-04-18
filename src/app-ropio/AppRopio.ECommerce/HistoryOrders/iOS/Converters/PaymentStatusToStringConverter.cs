using System;
using System.Globalization;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Platform.Converters;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross.Platform;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Converters
{
    public class PaymentStatusToStringConverter : MvxValueConverter<PaymentStatus, string>
    {
        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();
        
        protected override string Convert(PaymentStatus value, Type targetType, object parameter, CultureInfo culture)
		{
            switch (value)
            {
                case PaymentStatus.Paid:
                    return LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "Status_Paid");

                case PaymentStatus.NotPaid:
                    return LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "Status_NotPaid");
                default:
                    return null;
            }
		}
    }
}
using Android.Views;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross.Converters;

namespace AppRopio.ECommerce.HistoryOrders.Core.ValueConverters
{
    public class PaymentStatusVisibilityValueConverter : MvxValueConverter<PaymentStatus, ViewStates>
    {
        protected override ViewStates Convert(PaymentStatus value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == PaymentStatus.Unknown ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}

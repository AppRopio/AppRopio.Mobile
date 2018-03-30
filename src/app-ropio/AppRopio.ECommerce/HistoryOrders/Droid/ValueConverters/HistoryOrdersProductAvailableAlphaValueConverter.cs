using System;
using MvvmCross.Platform.Converters;
namespace AppRopio.ECommerce.HistoryOrders.Droid.ValueConverters
{
    public class HistoryOrdersProductAvailableAlphaValueConverter : MvxValueConverter<bool, float>
    {
        protected override float Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ? 1f : 0.5f;
        }
    }
}

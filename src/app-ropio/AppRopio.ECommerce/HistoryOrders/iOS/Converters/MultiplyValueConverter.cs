using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Converters
{
    public class MultiplyValueConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = System.Convert.ToDouble(value);
            var multiplier = System.Convert.ToDouble(parameter);

            return v * multiplier;
        }
    }
}
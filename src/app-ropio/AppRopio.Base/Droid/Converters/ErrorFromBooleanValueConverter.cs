using System;
using MvvmCross.Platform.Converters;
namespace AppRopio.Base.Droid.Converters
{
    public class ErrorFromBooleanValueConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ? string.Empty : " ";
        }
    }
}

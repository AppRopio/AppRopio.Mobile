using System;
using MvvmCross.Converters;

namespace AppRopio.Base.Core.Converters
{
    public class TrueFalseConverter : MvxValueConverter<bool>
    {
        protected override object Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var trueFalseParameter = parameter as TrueFalseParameter;
            if (trueFalseParameter == null)
                return null;

            return value ? trueFalseParameter.True : trueFalseParameter.False;
        }
    }
}

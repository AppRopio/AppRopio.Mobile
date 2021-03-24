using System;
using System.Globalization;
using System.Text.RegularExpressions;
using MvvmCross.Converters;

namespace AppRopio.Base.Core.Converters
{
    public class StringFormatConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is StringFormatParameter)
                return string.Format((parameter as StringFormatParameter).StringFormat(value) ?? "{0}", value).Trim();

            if (value == null)
                return null;
            
            string result;
            try
            {
                result = string.Format(parameter?.ToString() ?? "{0}", value).Trim();
            }
            catch
            {
                result = value.ToString().Trim();
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value.ToString();

            DateTimeOffset dateTimeVal;
            if (DateTimeOffset.TryParse(str, out dateTimeVal))
                return dateTimeVal;

            double doubleVal = 0;
            if (double.TryParse(Regex.Replace(str, @"\D+", ""), out doubleVal))
                return doubleVal;

            decimal decVal = 0;
            if (decimal.TryParse(Regex.Replace(str, @"\D+", ""), out decVal))
                return decVal;

            int intVal = 0;
            if (int.TryParse(Regex.Replace(str, @"\D+", ""), out intVal))
                return intVal;

            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}

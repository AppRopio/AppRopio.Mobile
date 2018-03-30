using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace AppRopio.Feedback.iOS.Converters
{
    public class DateToStringConverter : MvxValueConverter<DateTime, string>
	{
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
            return value.ToString(parameter.ToString(), Culture);
		}
	}
}
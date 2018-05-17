using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using AppRopio.Base.Core;

namespace AppRopio.Base.Core.Converters
{
    public class DateToStringConverter : MvxValueConverter<DateTime, string>
	{
        public CultureInfo Culture { get; set; } = AppSettings.SettingsCulture;

        protected override string Convert(DateTime value, Type targetType, object parameter, CultureInfo culture)
		{
            return value.ToString(parameter.ToString(), Culture);
		}
	}
}
﻿using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace AppRopio.Base.Core.Converters
{
    public class PriceFormatConverter : IMvxValueConverter
    {
        private NumberFormatInfo _defaultFormat = (NumberFormatInfo)AppSettings.SettingsCulture.NumberFormat.Clone();

        public string CurrencyFormat { get; set; } = "C0";

		public string CurrencySymbol { get; set; } = AppSettings.SettingsCulture.NumberFormat.CurrencySymbol;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            
            var price = System.Convert.ToDecimal(value);
            var format = parameter as NumberFormatInfo ?? _defaultFormat;

            if (!string.IsNullOrEmpty(CurrencySymbol))
                format.CurrencySymbol = CurrencySymbol;

            return price.ToString(CurrencyFormat, format);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
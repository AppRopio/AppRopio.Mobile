using System;
using System.Globalization;
using System.Linq;
using MvvmCross.Converters;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross;

namespace AppRopio.Base.Core.Converters
{
    public class PriceFormatUnitConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            var price = System.Convert.ToDecimal(value);
            var priceString = string.Empty;

            var unitParameter = parameter as PriceFormatUnitParameter ?? new PriceFormatUnitParameter();

            var format = unitParameter.Format;

            if (!string.IsNullOrEmpty(unitParameter.CurrencySymbol))
                format.CurrencySymbol = unitParameter.CurrencySymbol;

            if (!unitParameter.PrefixString.IsNullOrEmpty())
                priceString += unitParameter.PrefixString;

            priceString += price.ToString(unitParameter.CurrencyFormat, format);

            if (!unitParameter.UnitName.IsNullOrEmpty())
                priceString += $"/{unitParameter.UnitName}";

            if (parameter is string strFormating)
                return string.Format(strFormating, priceString);

            return priceString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PriceFormatUnitParameter
    {
        public NumberFormatInfo Format { get; set; } = (NumberFormatInfo)AppSettings.SettingsCulture.NumberFormat.Clone();

        public string CurrencyFormat { get; set; } = AppSettings.CurrencyFormat;

        public string CurrencySymbol { get; set; } = AppSettings.SettingsCulture.NumberFormat.CurrencySymbol;

        public string PrefixString { get; set; }

        public string UnitName { get; set; }
    }

    public class ParsePriceFormatUnitParameterConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priceParameter = new PriceFormatUnitParameter();

            if (value is string str && !str.IsNullOrEmtpy())
            {
                var paramsDictionary = str.Split(',')
                                          .ToDictionary(
                                              x => x.Split('=')[0],
                                              x => x.Split('=')[1]
                                          );

                if (paramsDictionary.ContainsKey("Format"))
                    priceParameter.Format = (NumberFormatInfo)new CultureInfo(paramsDictionary["Format"]).NumberFormat.Clone();

                if (paramsDictionary.ContainsKey("CurrencyFormat"))
                    priceParameter.CurrencyFormat = paramsDictionary["CurrencyFormat"];

                if (paramsDictionary.ContainsKey("CurrencySymbol"))
                    priceParameter.CurrencySymbol = paramsDictionary["CurrencySymbol"];

                if (paramsDictionary.ContainsKey("PrefixString"))
                    priceParameter.PrefixString = paramsDictionary["PrefixString"];

                if (paramsDictionary.ContainsKey("UnitName"))
                    priceParameter.UnitName = paramsDictionary["UnitName"];

                if (paramsDictionary.ContainsKey("ResxName") && paramsDictionary.ContainsKey("StringName"))
                    priceParameter.PrefixString = Mvx.Resolve<ILocalizationService>().GetLocalizableString(paramsDictionary["ResxName"], paramsDictionary["StringName"]);
            }

            return priceParameter;
        }
    }
}

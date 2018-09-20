using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MvvmCross.Binding.Combiners;

namespace AppRopio.Base.Core.Combiners
{
    public class PriceUnitCombiner : MvxValueCombiner
    {
        private readonly NumberFormatInfo _defaultFormat = (NumberFormatInfo)AppSettings.SettingsCulture.NumberFormat.Clone();

        public string CurrencyFormat { get; set; } = AppSettings.CurrencyFormat;

        public string CurrencySymbol { get; set; } = AppSettings.SettingsCulture.NumberFormat.CurrencySymbol;

        public override bool TryGetValue(IEnumerable<MvvmCross.Binding.Bindings.SourceSteps.IMvxSourceStep> steps, out object value)
        {
            var priceString = string.Empty;

            var values = steps.ToList();

            var priceValue = values.FirstOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
            if (priceValue != null)
            {
                if (priceValue.SourceType == typeof(Decimal))
                {
                    var price = Convert.ToDecimal(priceValue.GetValue());
                    priceString += price.ToString(CurrencyFormat, _defaultFormat);
                }
                else if (priceValue.SourceType == typeof(Decimal?))
                {
                    var price = (decimal?)priceValue.GetValue();
                    if (price.HasValue)
                        priceString += price.Value.ToString(CurrencyFormat, _defaultFormat);
                }
            }

            var unitValue = values.FirstOrDefault(x => x.SourceType == typeof(String));
            if (unitValue != null)
            {
                var unitName = (string)unitValue.GetValue();
                if (!unitName.IsNullOrEmpty())
                    priceString += $"/{unitName}";
            }

            value = priceString;

            return true;
        }
    }
}

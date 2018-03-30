using System;
using System.Globalization;
using System.Linq;
using MvvmCross.Binding.Combiners;
using System.Collections.Generic;

namespace AppRopio.Base.Core.Combiners
{
    public class PriceUnitCombiner : MvxValueCombiner
    {
        public override bool TryGetValue(System.Collections.Generic.IEnumerable<MvvmCross.Binding.Bindings.SourceSteps.IMvxSourceStep> steps, out object value)
        {
            var priceString = string.Empty;

            var values = steps.ToList();

            var priceValue = values.FirstOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
            if (priceValue != null)
            {
                if (priceValue.SourceType == typeof(Decimal))
                {
                    var price = Convert.ToDecimal(priceValue.GetValue());
                    priceString += price.ToString("C0", (NumberFormatInfo)new CultureInfo("ru").NumberFormat.Clone());
                }
                else if (priceValue.SourceType == typeof(Decimal?))
                {
                    var price = (decimal?)priceValue.GetValue();
                    if (price.HasValue)
                        priceString += price.Value.ToString("C0", (NumberFormatInfo)new CultureInfo("ru").NumberFormat.Clone());
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

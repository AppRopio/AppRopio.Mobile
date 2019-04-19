using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;
using AppRopio.Base.Core.Converters;
using AppRopio.ECommerce.Products.Core.Models;
using MvvmCross.Platform;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Base.Core.Services.Localization;

namespace AppRopio.ECommerce.Products.Core.Combiners
{
    public class PriceFromFormatCombiner : MvxValueCombiner
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        private ILocalizationService _localizationService => Mvx.Resolve<ILocalizationService>();

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            var priceString = string.Empty;
            var values = steps.ToList();

            var priceValue = values.FirstOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
            if (priceValue != null)
            {
                object priceFormatted = null;
                var priceFormatConverter = new PriceFormatConverter();
                if (priceValue.SourceType == typeof(Decimal)) {
                    var price = Convert.ToDecimal(priceValue.GetValue());
                    priceFormatted = priceFormatConverter.Convert(price, typeof(Decimal), null, null);
                } else if (priceValue.SourceType == typeof(Decimal?)) {
                    var price = (decimal?)priceValue.GetValue();
                    if (price.HasValue)
                        priceFormatted = priceFormatConverter.Convert(price, typeof(Decimal), null, null);
                }
                priceString += priceFormatted ?? string.Empty;
            }

            if (_config.PriceType == PriceType.Default || _config.PriceType == PriceType.To) {
                value = priceString;
                return true;
            }

            var maxPriceValue = values.LastOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
            if (maxPriceValue != null && maxPriceValue != priceValue)
            {
                bool format = false;
                if (maxPriceValue.SourceType == typeof(Decimal)) {
                    format = true;
                } else if (maxPriceValue.SourceType == typeof(Decimal?)) {
                    var price = (decimal?)maxPriceValue.GetValue();
                    format = price.HasValue;
                }
                if (format)
                    priceString = $"{_localizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Catalog_PriceFrom")} {priceString}";
            }

            value = priceString;
            return true;
        }
    }
}

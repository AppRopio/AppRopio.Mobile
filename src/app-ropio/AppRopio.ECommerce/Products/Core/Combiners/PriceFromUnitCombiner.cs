using System;
using System.Collections.Generic;
using System.Linq;
using AppRopio.Base.Core.Combiners;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.Combiners
{
    public class PriceFromUnitCombiner : PriceUnitCombiner
    {
        private ProductsConfig _config => Mvx.IoCProvider.Resolve<IProductConfigService>().Config;

        private ILocalizationService _localizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value)
        {
            bool ret = base.TryGetValue(steps, out value);

            if (_config.PriceType == PriceType.Default || _config.PriceType == PriceType.To) {
                return true;
            }

            var priceString = value ?? string.Empty;
            var values = steps.ToList();

            var priceValue = values.FirstOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
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
            return ret;
        }
    }
}

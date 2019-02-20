using System;
using System.Collections.Generic;
using System.Linq;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Binding.Combiners;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace AppRopio.ECommerce.Products.Core.Combiners
{
    public class PriceVisibilityValueCombiner : MvxValueCombiner
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value) {
            var values = steps.ToList();

            bool oldPriceValid = false;

            var oldPriceValue = values.FirstOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
            if (oldPriceValue != null) {
                if (oldPriceValue.SourceType == typeof(Decimal)) {
                    oldPriceValid = true;
                } else if (oldPriceValue.SourceType == typeof(Decimal?)) {
                    var price = (decimal?)oldPriceValue.GetValue();
                    oldPriceValid = price.HasValue;
                }
            }

            if (_config.PriceType == PriceType.FromTo || _config.PriceType == PriceType.To) {
                bool maxPriceValid = false;

                var maxPriceValue = values.LastOrDefault(x => x.SourceType == typeof(Decimal) || x.SourceType == typeof(Decimal?));
                if (maxPriceValue != null && maxPriceValue != oldPriceValue) {
                    if (maxPriceValue.SourceType == typeof(Decimal)) {
                        maxPriceValid = true;
                    } else if (maxPriceValue.SourceType == typeof(Decimal?)) {
                        var price = (decimal?)maxPriceValue.GetValue();
                        maxPriceValid = price.HasValue;
                    }
                }

                if (maxPriceValid) {
                    value = MvxVisibility.Collapsed;
                    return true;
                }
            }

            value = oldPriceValid ? MvxVisibility.Visible : MvxVisibility.Hidden;
            return true;
        }
    }
}

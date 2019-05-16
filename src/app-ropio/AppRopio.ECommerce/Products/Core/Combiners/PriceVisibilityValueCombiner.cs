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
    public class PriceVisibilityCommonValueCombiner : MvxValueCombiner
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value) {
            var values = steps.ToList();

            bool priceValid = false;

            var priceValue = values.FirstOrDefault(x => x.SourceType == typeof(String));
            if (priceValue != null) {
                if (priceValue.SourceType == typeof(String)) {
                    var price = (String)priceValue.GetValue();
                    priceValid = !string.IsNullOrEmpty(price);
                }
            }

            if (_config.PriceType == PriceType.FromTo || _config.PriceType == PriceType.To) {
                bool maxPriceValid = false;

                var maxPriceValue = values.LastOrDefault(x => x.SourceType == typeof(String));
                if (maxPriceValue != null && maxPriceValue != priceValue) {
                    if (maxPriceValue.SourceType == typeof(String)) {
                        var price = (String)maxPriceValue.GetValue();
                        maxPriceValid = !string.IsNullOrEmpty(price);
                    }
                }

                if (maxPriceValid) {
                    value = MvxVisibility.Collapsed;
                    return true;
                }
            }

            value = priceValid ? MvxVisibility.Visible : MvxVisibility.Hidden;
            return true;
        }
    }
}

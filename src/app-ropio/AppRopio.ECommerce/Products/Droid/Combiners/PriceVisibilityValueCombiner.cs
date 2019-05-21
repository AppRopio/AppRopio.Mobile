using System;
using System.Collections.Generic;
using System.Globalization;
using Android.Views;
using AppRopio.ECommerce.Products.Core.Combiners;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using MvvmCross.Binding.Bindings.SourceSteps;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace AppRopio.ECommerce.Products.Droid.Combiners
{
    public class OldPriceVisibilityValueCombiner : PriceVisibilityCommonValueCombiner
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value) {
            var ret = base.TryGetValue(steps, out value);
            var visibility = (MvxVisibility)value;
            switch (visibility) {
                case MvxVisibility.Collapsed:
                    value = ViewStates.Gone;
                    break;
                case MvxVisibility.Hidden:
                    value = ViewStates.Invisible;
                    break;
                default:
                    value = ViewStates.Visible;
                    break;
            }
            return ret;
        }
    }

    public class PriceVisibilityValueCombiner : OldPriceVisibilityValueCombiner
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        public override bool TryGetValue(IEnumerable<IMvxSourceStep> steps, out object value) {
            if (_config.PriceType == PriceType.FromTo) {
                value = ViewStates.Visible;
                return true;
            }
            return base.TryGetValue(steps, out value);
        }
    }

    public class MaxPriceVisibilityValueConverter : MvxValueConverter<string, ViewStates>
    {
        private ProductsConfig _config => Mvx.Resolve<IProductConfigService>().Config;

        protected override ViewStates Convert(string value, Type targetType, object parameter, CultureInfo culture) {
            if (!(_config.PriceType == PriceType.To || _config.PriceType == PriceType.FromTo))
                return ViewStates.Gone;
            return string.IsNullOrEmpty(value) ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}

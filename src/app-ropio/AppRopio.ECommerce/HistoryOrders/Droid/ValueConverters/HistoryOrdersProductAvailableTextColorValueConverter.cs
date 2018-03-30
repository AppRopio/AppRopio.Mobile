using System;
using MvvmCross.Platform.Converters;
using Android.Graphics;
using MvvmCross.Platform.Droid.Platform;
using Android.Util;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.HistoryOrders.Droid.ValueConverters
{
    public class HistoryOrdersProductAvailableTextColorValueConverter : MvxValueConverter<bool, Color>
    {
        protected override Color Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var typedValue = new TypedValue();
            if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity?.Theme.ResolveAttribute((value ? Resource.Attribute.app_color_textBase : Resource.Attribute.app_color_textGray), typedValue, true) ?? false)
                return new Color(typedValue.Data);

            return Color.Transparent;
        }
    }
}

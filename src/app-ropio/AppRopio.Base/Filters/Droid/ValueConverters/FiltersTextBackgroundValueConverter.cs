using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;

namespace AppRopio.Base.Filters.Droid.ValueConverters
{
    public class FiltersTextBackgroundValueConverter : MvxValueConverter<bool, Drawable>
    {
        protected override Drawable Convert(bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var drawable = new GradientDrawable();

            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_filters_filters_horizontalCollection_text_radius));

            var typedValue = new TypedValue();
            if (Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity?.Theme.ResolveAttribute((value ? Resource.Attribute.app_color_accent : Resource.Attribute.app_color_disabledControl), typedValue, true) ?? false)
                drawable.SetColor(new Color(typedValue.Data));

            return drawable;
        }
    }
}

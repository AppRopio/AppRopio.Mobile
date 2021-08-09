using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using MvvmCross.Converters;

namespace AppRopio.ECommerce.Products.Droid.ValueConverters
{
    public class BadgeBackgroundValueConverter : MvxValueConverter<string, Drawable>
    {
        protected override Drawable Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var drawable = new GradientDrawable();

            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_products_catalog_item_badge_cornerRadius));
            drawable.SetColor(Color.ParseColor(value));

            return drawable;
        }
    }
}

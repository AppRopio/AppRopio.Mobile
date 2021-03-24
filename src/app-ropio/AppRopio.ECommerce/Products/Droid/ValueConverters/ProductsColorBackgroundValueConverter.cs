using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using MvvmCross.Converters;

namespace AppRopio.ECommerce.Products.Droid.ValueConverters
{
    public class ProductsColorBackgroundValueConverter : MvxValueConverter<string, Drawable>
    {
        protected override Drawable Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var drawable = new GradientDrawable();

            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_products_productCard_horizontalCollection_color_radius));
            drawable.SetColor(Color.ParseColor(value));

            return drawable;
        }
    }
}

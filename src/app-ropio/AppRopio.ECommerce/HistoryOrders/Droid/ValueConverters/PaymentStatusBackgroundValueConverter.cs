using System;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Android;

namespace AppRopio.ECommerce.HistoryOrders.Droid.ValueConverters
{
    public class PaymentStatusBackgroundValueConverter : MvxValueConverter<PaymentStatus, Drawable>
    {
        protected override Drawable Convert(PaymentStatus value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var drawable = new GradientDrawable();

            drawable.SetShape(ShapeType.Rectangle);
            drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_historyorders_item_badge_cornerRadius));

            var typedValue = new TypedValue();
            if (Mvx.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity?.Theme.ResolveAttribute((value == PaymentStatus.Paid ? Resource.Attribute.app_color_accent : Resource.Attribute.app_color_disabledControl), typedValue, true) ?? false)
                drawable.SetColor(new Color(typedValue.Data));

            return drawable;
        }
    }
}

using System;
using MvvmCross.Platform.Converters;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;
namespace AppRopio.Base.iOS.ValueConverters
{
    public class ColorValueConverter : MvxValueConverter<string, UIColor>
    {
        protected override UIColor Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToUIColor();
        }
    }
}

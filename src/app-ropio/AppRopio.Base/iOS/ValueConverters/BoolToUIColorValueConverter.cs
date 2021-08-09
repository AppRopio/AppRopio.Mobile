using System;
using System.Globalization;
using AppRopio.Base.iOS.Models.ValueConverters;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Converters;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class BoolToUIColorValueConverter : MvxValueConverter<bool, UIColor>
    {
        protected override UIColor Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
			var boolToUiColorParameter = parameter as BoolToUIColorParameter;

			return boolToUiColorParameter == null ?
				UIColor.Clear :
		       (value ? boolToUiColorParameter.TrueColor : boolToUiColorParameter.FalseColor);
        }
    }
}

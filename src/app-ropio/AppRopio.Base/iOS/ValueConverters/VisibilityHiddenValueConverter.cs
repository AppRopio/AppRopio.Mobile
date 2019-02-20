using System;
using System.Globalization;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform.UI;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class VisibilityHiddenValueConverter : MvxValueConverter<MvxVisibility, bool>
    {
        protected override bool Convert(MvxVisibility value, Type targetType, object parameter, CultureInfo culture) {
            return value != MvxVisibility.Visible;
        }
    }
}

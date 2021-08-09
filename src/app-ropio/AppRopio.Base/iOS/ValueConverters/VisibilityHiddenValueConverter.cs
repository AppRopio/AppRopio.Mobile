using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.UI;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class VisibilityHiddenValueConverter : MvxValueConverter<MvxVisibility, bool>
    {
        protected override bool Convert(MvxVisibility value, Type targetType, object parameter, CultureInfo culture) {
            return value != MvxVisibility.Visible;
        }
    }
}

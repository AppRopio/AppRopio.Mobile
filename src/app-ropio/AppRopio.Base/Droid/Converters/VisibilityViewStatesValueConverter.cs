using System;
using System.Globalization;
using Android.Views;
using MvvmCross.Converters;
using MvvmCross.UI;

namespace AppRopio.Base.Droid.Converters
{
    public class VisibilityViewStatesValueConverter : MvxValueConverter<MvxVisibility, ViewStates>
    {
        protected override ViewStates Convert(MvxVisibility value, Type targetType, object parameter, CultureInfo culture) {
            switch (value) {
                case MvxVisibility.Collapsed:
                    return ViewStates.Gone;
                case MvxVisibility.Hidden:
                    return ViewStates.Invisible;
                default:
                    return ViewStates.Visible;
            }
        }
    }
}

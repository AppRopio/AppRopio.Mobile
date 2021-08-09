using System;
using System.Linq;
using AppRopio.Base.iOS.Models.ValueConverters;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Converters;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class SizeVisibilityValueConverter : MvxValueConverter<int, UIView>
    {
        protected override UIView Convert(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(parameter is SizeVisibilityParameter))
                return null;

            var sizeParam = ((SizeVisibilityParameter)parameter);
            if (!System.Convert.ToBoolean(value))
            {
                var consts = sizeParam.View.Constraints;

                if (sizeParam.MinimumHeight != null)
                {
                    var height = consts.FirstOrDefault(x => x.FirstAttribute.Equals(NSLayoutAttribute.Height));
                    if (height != null)
                        height.Constant = sizeParam.MinimumHeight.Invoke();
                    else
                        sizeParam.View.ChangeFrame(h: sizeParam.MinimumHeight.Invoke());
                }

                if (sizeParam.MinimumWidth != null)
                {
                    var width = consts.FirstOrDefault(x => x.FirstAttribute.Equals(NSLayoutAttribute.Width));
                    if (width != null)
                        width.Constant = sizeParam.MinimumWidth.Invoke();
                    else
                        sizeParam.View.ChangeFrame(h: sizeParam.MinimumWidth.Invoke());
                }
            }
            else
            {
                var consts = sizeParam.View.Constraints;

                if (sizeParam.MaximumHeight != null)
                {
                    var height = consts.FirstOrDefault(x => x.FirstAttribute.Equals(NSLayoutAttribute.Height));
                    if (height != null)
                        height.Constant = sizeParam.MaximumHeight.Invoke();
                    else
                        sizeParam.View.ChangeFrame(h: sizeParam.MaximumHeight.Invoke());
                }

                if (sizeParam.MaximumWidth != null)
                {
                    var width = consts.FirstOrDefault(x => x.FirstAttribute.Equals(NSLayoutAttribute.Width));
                    if (width != null)
                        width.Constant = sizeParam.MaximumWidth.Invoke();
                    else
                        sizeParam.View.ChangeFrame(h: sizeParam.MaximumWidth.Invoke());
                }
            }

            UIApplication.SharedApplication.InvokeOnMainThread(() => sizeParam.View.LayoutIfNeeded());

            return sizeParam.View;
        }
    }
}

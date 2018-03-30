using System;
using UIKit;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UISwitch view, Switch model)
        {
            if (model.OnTintColor != null)
                view.OnTintColor = model.OnTintColor.ToUIColor();
        }
    }
}

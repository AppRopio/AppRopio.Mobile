using System;
using UIKit;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle (this UIView self, View model)
        {
            if (model == null)
                return;

            self.BackgroundColor = model.Background?.ToUIColor() ?? UIColor.Clear;

            if (model.Layer != null)
                self.Layer.SetupStyle(model.Layer);
        }
    }
}

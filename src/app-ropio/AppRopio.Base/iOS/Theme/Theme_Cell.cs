using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UICollectionViewCell self, View model)
        {
            if (model == null)
                return;

            if (model.Layer != null)
            {
                self.Layer.SetupStyle(model.Layer);

                if (self.ContentView != null)
                    self.ContentView.Layer.SetupStyle(null, model.Layer.Border, model.Layer.CornerRadius, true, model.Layer.Background);
            }

            self.BackgroundColor = model.Background?.ToUIColor() ?? UIColor.Clear;
        }

        public static void SetupStyle(this UITableViewCell self, View model)
        {
            if (model == null)
                return;

            if (model.Layer != null)
            {
                self.Layer.SetupStyle(model.Layer);

                if (self.ContentView != null)
                    self.ContentView.Layer.SetupStyle(null, model.Layer.Border, model.Layer.CornerRadius, true, model.Layer.Background);
            }

            self.BackgroundColor = model.Background?.ToUIColor() ?? UIColor.Clear;
        }
    }
}

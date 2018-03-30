using AppRopio.Base.iOS.UIExtentions;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UINavigationBar view, Models.ThemeConfigs.NavigationBar model)
        {
            view.BarTintColor = model.BackgroundColor.ToUIColor();
            view.TintColor = model.TintColor.ToUIColor();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                view.PrefersLargeTitles = model.PrefersLargeTitles;

                if (model.LargeTitle != null && view.PrefersLargeTitles)
                    view.LargeTitleTextAttributes = new UIStringAttributes
                    {
                        Font = (UIFont)model.LargeTitle.Font ?? UIFont.GetPreferredFontForTextStyle(UIFontTextStyle.LargeTitle),
                        ForegroundColor = model.LargeTitle.TextColor.ToUIColor(),
                        KerningAdjustment = model.LargeTitle.Kerning ?? 0
                    };

                view.Opaque = model.Opaque;
                view.Translucent = model.Translucent;

                if (!model.Translucent)
                {
                    view.SetBackgroundImage(model.BackgroundColor.ToUIImage(), UIBarPosition.Any, UIBarMetrics.Default);
                    view.ShadowImage = model.BackgroundColor.ToUIImage();
                }
                else
                {
                    view.BackgroundColor = model.BackgroundColor.ToUIColor();
                    view.ShadowImage = new UIImage(); 
                    view.SetBackgroundImage(null, UIBarMetrics.Default);
                }
            }
            else
            {
                view.SetBackgroundImage(model.BackgroundColor.ToUIImage(), UIBarPosition.Any, UIBarMetrics.Default);
                view.ShadowImage = model.BackgroundColor.ToUIImage();
                view.Translucent = false;
                view.Opaque = false;
            }

            if (model.Title != null)
                view.TitleTextAttributes = new UIStringAttributes
                {
                    Font = (UIFont)model.Title.Font,
                    ForegroundColor = model.Title.TextColor.ToUIColor(),
                    KerningAdjustment = model.Title.Kerning ?? 0
                };
        }
    }
}

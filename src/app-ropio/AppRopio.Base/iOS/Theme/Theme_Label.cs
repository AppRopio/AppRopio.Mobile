using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.UIExtentions;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UILabel view, Models.ThemeConfigs.Label model)
        {
            view.Font = (UIFont)model.Font;
            view.TextColor = model.TextColor.ToUIColor();
            view.TextAlignment = model.TextAlignment.ToUITextAlignment();

            if (model.HighlightedTextColor != null)
                view.HighlightedTextColor = model.HighlightedTextColor.ToUIColor();

            if (model.Background != null)
                view.BackgroundColor = model.Background.ToUIColor();

            if (model.Layer != null)
                view.Layer.SetupStyle(model.Layer);

            var arLabel = view as ARLabel;
            if (arLabel != null)
            {
                arLabel.TextTransform = model.TextTransform;
                arLabel.TextDecoration = model.TextDecoration;
                arLabel.Kerning = model.Kerning;
            }
        }
    }
}

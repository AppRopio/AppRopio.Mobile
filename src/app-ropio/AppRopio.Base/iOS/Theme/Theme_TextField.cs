using UIKit;
using Foundation;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Controls;

namespace AppRopio.Base.iOS
{

    public static partial class Theme
    {
        public static void SetupStyle(this UITextField view, Models.ThemeConfigs.TextField model)
        {
            view.Font = (UIFont)model.Font;
            view.AttributedPlaceholder = new NSMutableAttributedString(view.Placeholder ?? " ", (UIFont)model.Placeholder.Font, (UIColor)model.Placeholder.TextColor, kerning: model.Placeholder.Kerning ?? 0);
            view.TextColor = (UIColor)model.TextColor;
            view.TextAlignment = model.TextAlignment.ToUITextAlignment();
            view.TintColor = (UIColor)model.TintColor;

            if (model.Layer != null)
                view.Layer.SetupStyle(model.Layer);

            if (view is ARTextField arTextField)
            {
                arTextField.SeparatorColor = (UIColor)model.SeparatorColor;
                arTextField.InvalidTintColor = (UIColor)model.InvalidTintColor;
                arTextField.TextTransform = model.TextTransform;
                arTextField.Kerning = model.Kerning;
            }
        }

        public static void SetupStyle(this ARTextField view, Models.ThemeConfigs.TextField model)
        {
            view.Font = (UIFont)model.Font;
            view.AttributedPlaceholder = new NSMutableAttributedString(view.Placeholder ?? " ", (UIFont)model.Placeholder.Font, (UIColor)model.Placeholder.TextColor, kerning: model.Placeholder.Kerning ?? 0);
            view.TextColor = (UIColor)model.TextColor;
            view.TextAlignment = model.TextAlignment.ToUITextAlignment();
            view.TintColor = (UIColor)model.TintColor;

            view.SeparatorColor = (UIColor)model.SeparatorColor;
            view.InvalidTintColor = (UIColor)model.InvalidTintColor;
            view.TextTransform = model.TextTransform;
            view.Kerning = model.Kerning;

            if (model.Layer != null)
                view.Layer.SetupStyle(model.Layer);
        }
    }
}

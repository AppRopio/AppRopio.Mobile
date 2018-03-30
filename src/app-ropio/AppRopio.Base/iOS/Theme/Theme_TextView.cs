using System;
using AppRopio.Base.iOS.UIExtentions;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this UITextView view, Models.ThemeConfigs.TextView model)
        {
            view.Font = (UIFont)model.Font;
            view.TextColor = (UIColor)model.TextColor;
            view.TextAlignment = model.TextAlignment.ToUITextAlignment();
            view.TintColor = (UIColor)model.TintColor;

            if (model.Background != null)
                view.BackgroundColor = model.Background.ToUIColor();

            if (model.Layer != null)
                view.Layer.SetupStyle(model.Layer);

            //TODO нужен еще кастомный контрол с плейсхолдером
        }
    }
}

using System;
using CoreAnimation;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static void SetupStyle(this CALayer layer, Shadow shadow, Border border, nfloat? cornerRadius, bool? maskToBounds, Color background)
        {
            if (border != null)
            {
                layer.BorderWidth = border.Width;
                layer.BorderColor = border.Color.ToUIColor().CGColor;
            }

            if (cornerRadius.HasValue)
                layer.CornerRadius = cornerRadius.Value;

            if (maskToBounds != null && maskToBounds.HasValue)
                layer.MasksToBounds = maskToBounds.Value;

            if (shadow != null && shadow.Color != null)
            {
                layer.ShadowColor = shadow.Color.ToUIColor().CGColor;
                layer.ShadowOffset = new CoreGraphics.CGSize(shadow.X, shadow.Y);
                layer.ShadowRadius = shadow.Blur;
                layer.ShadowOpacity = shadow.Opacity;
                layer.MasksToBounds = false;
            }

            if (background != null)
                layer.BackgroundColor = background.ToUIColor().CGColor;
        }

        public static void SetupStyle(this CALayer layer, Layer model)
        {
            if (model != null)
                SetupStyle(layer, model.Shadow, model.Border, model.CornerRadius, model.MaskToBounds, model.Background);
        }
    }
}

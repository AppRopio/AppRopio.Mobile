using System;
using CoreGraphics;
using MvvmCross.Converters;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class ColorMaskValueConverter : MvxValueConverter<UIImageView, UIImage>
    {
        protected override UIImage Convert(UIImageView value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var coloredImg = new UIImage();

            try
            {
                UIColor color = (parameter as UIColor);

                UIGraphics.BeginImageContextWithOptions(value.Image.Size, false, 0);
                var context = UIGraphics.GetCurrentContext();

                color.SetFill();

                context.TranslateCTM(0, value.Image.Size.Height);
                context.ScaleCTM(1, -1);

                context.SetBlendMode(CGBlendMode.ColorBurn);
                var rect = new CGRect(0, 0, value.Image.Size.Width, value.Image.Size.Height);
                context.DrawImage(rect, value.Image.CGImage);

                context.SetBlendMode(CGBlendMode.SourceIn);
                context.AddRect(rect);
                context.DrawPath(CGPathDrawingMode.Fill);

                coloredImg = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
            } 
            catch {}

            return coloredImg;
        }
    }
}

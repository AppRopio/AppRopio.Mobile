using System;
using CoreGraphics;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform.Converters;
using UIKit;

namespace AppRopio.Base.iOS.ValueConverters
{
    public class ColorMaskValueConverter : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.IsNullOrEmpty())
                return new UIImage();

            var coloredImg = new UIImage();

            try
            {
                var source = new MvxImageView { ImageUrl = value };
                UIColor color = (parameter as UIColor);

                UIGraphics.BeginImageContextWithOptions(source.Image.Size, false, 0);
                var context = UIGraphics.GetCurrentContext();

                color.SetFill();

                context.TranslateCTM(0, source.Image.Size.Height);
                context.ScaleCTM(1, -1);

                context.SetBlendMode(CGBlendMode.ColorBurn);
                var rect = new CGRect(0, 0, source.Image.Size.Width, source.Image.Size.Height);
                context.DrawImage(rect, source.Image.CGImage);

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

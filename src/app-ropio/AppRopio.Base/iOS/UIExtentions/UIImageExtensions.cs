using System;
using CoreGraphics;
using UIKit;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class UIImageExtensions
    {
        public static UIImage ApplyColorMask(this UIImage image, UIColor maskColor)
        {
            UIGraphics.BeginImageContextWithOptions(image.Size, false, 0);
            var context = UIGraphics.GetCurrentContext();

            maskColor.SetFill();

            context.TranslateCTM(0, image.Size.Height);
            context.ScaleCTM(1, -1);

            context.SetBlendMode(CGBlendMode.ColorBurn);
            var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);
            context.DrawImage(rect, image.CGImage);

            context.SetBlendMode(CGBlendMode.SourceIn);
            context.AddRect(rect);
            context.DrawPath(CGPathDrawingMode.Fill);

            var coloredImg = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return coloredImg;
        }
    }
}

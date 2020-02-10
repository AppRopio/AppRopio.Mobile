using System;
using UIKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using AppRopio.Base.iOS.Models.ThemeConfigs;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class UIColorExtensions
    {
        public static UIColor FromHex(this UIColor color, int hexValue)
        {
            try
            {
                return UIColor.FromRGB(
                    ((((hexValue & 0xFF0000) >> 16)) / 255.0f),
                    ((((hexValue & 0xFF00) >> 8)) / 255.0f),
                    (((hexValue & 0xFF)) / 255.0f)
                );
            }
            catch
            {
                return UIColor.Black;
            }
        }

        public static UIColor FromHex(this UIColor color, string hexValue)
        {
            hexValue = hexValue.Replace("#", "");
            if (hexValue.Length == 8)
            {
                var hexValueInt = Convert.ToInt64(hexValue, 16);
                return UIColor.FromRGBA(
                    ((((hexValueInt & 0xFF0000) >> 16)) / 255.0f),
                    ((((hexValueInt & 0xFF00) >> 8)) / 255.0f),
                    (((hexValueInt & 0xFF)) / 255.0f),
                    ((((hexValueInt & -16777216) >> 24)) / 255.0f)
                );
            }
            else
            {
                var hexValueInt = Convert.ToInt32(hexValue, 16);
                return UIColor.FromRGB(
                    ((((hexValueInt & 0xFF0000) >> 16)) / 255.0f),
                    ((((hexValueInt & 0xFF00) >> 8)) / 255.0f),
                    (((hexValueInt & 0xFF)) / 255.0f)
                );
            }
        }

        /// <summary>
        /// по умолчанию вернет черный цвет
        /// </summary>
        public static UIColor ToUIColor(this string source)
        {
            return source.ToUIColor(UIColor.Black);
        }

        public static UIColor ToUIColor(this Color source)
        {
            return (UIColor)source;
        }

        /// <summary>
        /// позволяет задать цвет по-умолчанию строкой
        /// </summary>
        public static UIColor ToUIColor(this string source, string defaultColor)
        {
            if (string.IsNullOrEmpty(source))
                return defaultColor.ToUIColor(UIColor.Clear);

            if (source.ToLower() == "clear")
                return UIColor.Clear;

            if (source.Contains("."))
            {
                return UIColor.FromPatternImage(UIImage.FromFile(source));
            }

            if (!string.IsNullOrEmpty(source))
            {
                return (UIColor.Clear).FromHex(source);
            }

            return defaultColor.ToUIColor(UIColor.Clear);
        }

        /// <summary>
        /// превращает из строки UIColor
        /// </summary>
        public static UIColor ToUIColor(this string imagePath, UIColor defaultColor)
        {
            if (string.IsNullOrEmpty(imagePath))
                return defaultColor;

            if (imagePath.ToLower() == "clear")
                return UIColor.Clear;

            if (imagePath.Contains("."))
            {
                return UIColor.FromPatternImage(UIImage.FromFile(imagePath));
            }

            if (!string.IsNullOrEmpty(imagePath))
            {
                if (!imagePath.Contains("."))
                    return (UIColor.Clear).FromHex(imagePath);
                else
                    return UIColor.FromPatternImage(UIImage.FromFile(imagePath));
            }
            return defaultColor;
        }



        /// <summary>
        /// Создает градиентный слой, по-умолчанию градиент идет сверху вниз, используется как customView.Layer.InsertSublayer(UIColorExtensions.GradientLayer(...), 0);
        /// </summary>
        /// <returns>Gradient Layer</returns>
        /// <param name = "layer"></param>
        /// <param name="firstColor">First color.</param>
        /// <param name="secondColor">Second color.</param>
        /// <param name="frame">Frame. Если не задано, берутся локальные координаты</param>
        /// <param name="cornerRadius">Закругление углов, по-умолчанию берется то, как определено у родителя</param>
        /// <param name="starPoint">Star point. Точка, с которой начинается градиент, если не задано - new PointF(0.5f, 0f)</param>
        /// <param name="endPoint">End point. Конечная точка градиента, если не задано - new PointF(0.5f, 1f)</param>
        public static void Gradient(this CALayer layer, UIColor firstColor, UIColor secondColor, CGRect frame = default(CGRect), nfloat cornerRadius = default(nfloat), CGPoint starPoint = default(CGPoint), CGPoint endPoint = default(CGPoint))
        {
            CAGradientLayer gradientLayer = new CAGradientLayer();
            gradientLayer.Colors = new[] { firstColor.CGColor, secondColor.CGColor };
            gradientLayer.Frame = frame == CGRect.Empty ? layer.Bounds : frame;
            if (starPoint != CGPoint.Empty)
                gradientLayer.StartPoint = starPoint;
            if (endPoint != CGPoint.Empty)
                gradientLayer.EndPoint = endPoint;
            gradientLayer.AnchorPointZ = 0f;
            gradientLayer.Locations = new NSNumber[] { 0, 1 };
            gradientLayer.CornerRadius = cornerRadius == default(nfloat) ? layer.CornerRadius : cornerRadius;

            layer.InsertSublayer(gradientLayer, 0);
        }

        /// <summary>
        /// Создает картинку, залитую указанным цветом. По умолчанию размер 1x2
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        public static UIImage ToUIImage(this UIColor color, int width = 1, int height = 2)
        {
            CGBitmapContext context = new CGBitmapContext(
                                          System.IntPtr.Zero, // data
                                          width,              // width
                                          height,             // height
                                          8,                          // bitsPerComponent
                                          4 * width,                      // bytesPerRow
                                          CGColorSpace.CreateDeviceRGB(), // colorSpace
                                          CGImageAlphaInfo.PremultipliedFirst);// bitmapInfo

            //set up colours etc
            CGColor backgroundColour = color.CGColor;

            //draw a rectangle
            context.SetFillColor(backgroundColour);
            context.FillRect(new CGRect(0, 0, width, height));
            UIImage returnedImage = UIImage.FromImage(context.ToImage());
            return returnedImage;
        }

        public static UIImage ToUIImageRounded(this UIColor color, float? cornerRadius = null)
        {
            if (cornerRadius == null || !cornerRadius.HasValue)
                return ToUIImage(color);

            var scale = UIScreen.MainScreen.Scale;

            int width = (int)(cornerRadius * scale) + (int)scale;
            int height = (int)(cornerRadius * scale);

            var view = new UIView().WithFrame(0, 0, width, height)
                                   .WithBackground(color)
                                   .WithTune(tune => tune.Layer.CornerRadius = cornerRadius.Value);

            UIImage result = null;

            UIGraphics.BeginImageContextWithOptions(view.Frame.Size, false, 0);
            {
                view.Layer.RenderInContext(UIGraphics.GetCurrentContext());
                result = UIGraphics.GetImageFromCurrentImageContext();
            }
            UIGraphics.EndImageContext();

            return result.CreateResizableImage(new UIEdgeInsets(0, cornerRadius.Value, 0, cornerRadius.Value));
        }

        public static UIImage ToUIImage(this Color source, int width = 1, int height = 2)
        {
            return source.ToUIColor().ToUIImage(width, height);
        }

        /// <summary>
        /// Создает картинку, залитую указанным цветом. По умолчанию размер 1x2
        /// Цвет по-умолчанию ЧЕРНЫЙ
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        public static UIImage ToUIImage(this string color, int width = 1, int height = 2)
        {
            return color.ToUIColor().ToUIImage(width, height);
        }

        /// <summary>
        /// Создает картинку, залитую указанным цветом. По умолчанию размер 1x2
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="defaultColor">Цвет по-умолчанию</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        public static UIImage ToUIImage(this string color, string defaultColor, int width = 1, int height = 2)
        {
            return color.ToUIColor(defaultColor).ToUIImage(width, height);
        }

        /// <summary>
        /// Создает картинку, залитую указанным цветом. По умолчанию размер 1x2
        /// </summary>
        /// <param name="color">Цвет</param>
        /// <param name="defaultColor">Цвет по-умолчанию</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        public static UIImage ToUIImage(this string color, UIColor defaultColor, int width = 1, int height = 2)
        {
            return color.ToUIColor(defaultColor).ToUIImage(width, height);
        }

        /// <summary>
        /// Получает изображение из байт массива
        /// </summary>
        /// <returns>The user interface image.</returns>
        /// <param name="data">Data.</param>
        public static UIImage ToUIImage(this byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            UIImage image = null;
            try
            {
                image = new UIImage(NSData.FromArray(data));
                data = null;
            }
            catch (Exception)
            {
                return null;
            }
            return image;
        }

        /// <summary>
        /// Из изображения в байт массив
        /// </summary>
        /// <returns>The NS data.</returns>
        /// <param name="image">Image.</param>
        public static byte[] ToNSData(this UIImage image)
        {

            if (image == null)
            {
                return null;
            }
            NSData data = null;

            try
            {
                data = image.AsPNG();
                return data.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
            }
        }

        public static UITextAlignment ToUITextAlignment(this TextAlignment self)
        {
            return (UITextAlignment)((long)self);
        }
    }
}


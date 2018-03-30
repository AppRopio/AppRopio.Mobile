using System;
using System.Globalization;
using Foundation;
using MvvmCross.Platform.Converters;
using UIKit;

namespace AppRopio.Base.Contacts.iOS.Converters
{
    /// <summary>
    /// Конвертирует изображение в формате Base64 в UIImage
    /// </summary>
    public class Base64ImageConverter : MvxValueConverter<string, UIImage>
    {
        protected override UIImage Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
			var base64 = (string)value;
			if (string.IsNullOrEmpty(base64))
				return null;

			var bytes = System.Convert.FromBase64String(base64);
			var data = NSData.FromArray(bytes);
			return UIImage.LoadFromData(data);
        }
    }
}

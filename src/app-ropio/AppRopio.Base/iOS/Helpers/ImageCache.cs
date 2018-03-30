using System;
using System.Collections.Generic;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;
namespace AppRopio.Base.iOS.Helpers
{
    public static class ImageCache
    {
        private static Dictionary<string, UIImage> _cache = new Dictionary<string, UIImage>();

        private static string GetImageKey(string path, UIColor mask)
        {
            return path + mask?.ToString();
        }

        public static UIImage GetImage(string path, UIColor mask = null)
        {
            if (path.IsNullOrEmtpy())
                return null;

            var image = new UIImage();

            var targetKey = GetImageKey(path, mask);
            if (_cache.TryGetValue(targetKey, out image))
                return image;

            image = path.LoadFromFile();
            if (image == null)
                return new UIImage();

            if (mask == UIColor.Clear)
                return image;

            if (mask != null)
                image = image.ApplyColorMask(mask);

            _cache.Add(targetKey, image);

            return image;
        }
    }
}

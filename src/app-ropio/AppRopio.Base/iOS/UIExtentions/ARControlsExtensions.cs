using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class ARControlsExtensions
    {
        public static string ApplyTransform (this string self, TextTransform transform)
        {
            if (string.IsNullOrEmpty(self))
                return self;
            
            switch (transform)
            {
                case TextTransform.Uppercase:
                    return self.ToUpperInvariant();
                case TextTransform.Lowercase:
                    return self.ToLowerInvariant();
                default:
                    return self;
            }
        }

    }
}

using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using UIKit;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static FontsPalette FontsPalette { get { return ThemeConfig.FontsPalette; } }

        public static class Fonts
        {
            public static UIFont RegularOfSize(nfloat size)
            {
                return UIFont.FromName(FontsPalette.Regular, size);
            }

            public static UIFont LightOfSize(nfloat size)
            {
                return UIFont.FromName(FontsPalette.Light, size);
            }

            public static UIFont MediumOfSize(nfloat size)
            {
                return UIFont.FromName(FontsPalette.Medium, size);
            }

            public static UIFont SemiboldOfSize(nfloat size)
            {
                return UIFont.FromName(FontsPalette.Semibold, size);
            }

            public static UIFont BoldOfSize(nfloat size)
            {
                return UIFont.FromName(FontsPalette.Bold, size);
            }
        }
    }
}

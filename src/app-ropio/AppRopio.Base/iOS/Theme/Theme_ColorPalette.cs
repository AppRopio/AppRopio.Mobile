using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS
{
    public static partial class Theme
    {
        public static ColorPalette ColorPalette { get { return ThemeConfig.ColorPalette; } }
    }
}

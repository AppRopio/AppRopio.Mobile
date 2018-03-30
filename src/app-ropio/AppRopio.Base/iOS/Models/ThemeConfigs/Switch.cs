using System;
namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class Switch : ICloneable
    {
        public Color OnTintColor { get; set; }

        public object Clone()
        {
            return new Switch
            {
                OnTintColor = (Color)this.OnTintColor?.Clone()
            };
        }
    }
}

using System;
using System.Collections.Generic;

namespace AppRopio.Base.Settings.Core.Models
{
    public class SettingsConfig
    {
        public List<SettingsElement> Elements { get; set; }

        public SettingsConfig()
        {
            Elements = new List<SettingsElement>();
        }
    }
}
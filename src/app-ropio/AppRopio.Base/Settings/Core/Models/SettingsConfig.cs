using System;
using System.Collections.Generic;

namespace AppRopio.Base.Settings.Core.Models
{
    public class SettingsConfig
    {
        public List<SettingsElement> Elements { get; set; }

        public List<string> Languages { get; set; }

        public SettingsConfig()
        {
            Elements = new List<SettingsElement>();
            Languages = new List<string>();
        }
    }
}
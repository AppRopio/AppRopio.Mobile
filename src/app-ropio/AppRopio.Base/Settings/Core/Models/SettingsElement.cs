using System;
namespace AppRopio.Base.Settings.Core.Models
{
    public class SettingsElement
    {
        public SettingsElementType Type { get; set; }

        public string Title { get; set; }

        public bool IsEnabled { get; set; }
    }
}
using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Base.Contacts.iOS.Models
{
    public class ContactsThemeConfig
    {
        [JsonProperty("contactCell")]
        public ContactCell ContactCell { get; private set; }

        public ContactsThemeConfig()
        {
            ContactCell = new ContactCell();
        }
    }

    public class ContactCell : View
    {
        [JsonProperty("title")]
        public Label Title { get; private set; }

        [JsonProperty("size")]
        public Size Size { get; private set; }

        public ContactCell()
        {
            Title = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.SemiboldOfSize(14),
                HighlightedTextColor = (Color)Theme.ColorPalette.Accent.Clone()
            };

            Size = new Size() { Height = 51 };
        }
    }
}
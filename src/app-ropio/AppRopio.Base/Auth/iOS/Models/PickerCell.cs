using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Base.Auth.iOS.Models
{
	public class PickerCell
	{
		[JsonProperty("value")]
		public Label Value { get; private set; }

		[JsonProperty("size")]
		public Size Size { get; private set; }

		public PickerCell()
		{
            Value = new Label
            {
                TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16),
                TextAlignment = TextAlignment.Center
			};
			Size = new Size { Height = 30 };
		}
	}
}

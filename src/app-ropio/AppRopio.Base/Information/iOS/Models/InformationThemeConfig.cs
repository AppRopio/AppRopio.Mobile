using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;

namespace AppRopio.Base.Information.iOS.Models
{
    public class InformationThemeConfig
    {
        public InformationCell InformationCell { get; private set; }

		public InformationThemeConfig()
		{
			InformationCell = new InformationCell();
		}
    }

    public class InformationCell : View
	{
		public Size Size { get; set; }

		public Label Title { get; set; }

		public InformationCell()
		{
			Size = new Size() { Height = 52 };
			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};
		}
	}

}
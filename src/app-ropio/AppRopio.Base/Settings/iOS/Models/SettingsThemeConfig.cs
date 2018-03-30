using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using Newtonsoft.Json;

namespace AppRopio.Base.Settings.iOS.Models
{
    public class SettingsThemeConfig
    {
		[JsonProperty("cell")]
		public SettingsCell SettingsCell { get; private set; }

        [JsonProperty("regions")]
        public Regions Regions { get; set; }

        public SettingsThemeConfig()
        {
            SettingsCell = new SettingsCell();

            Regions = new Regions();
        }
    }

    public class SettingsCell : View
	{
		[JsonProperty("size")]
		public Size Size { get; set; }

		[JsonProperty("title")]
		public Label Title { get; set; }

		[JsonProperty("value")]
		public Label Value { get; set; }

		[JsonProperty("switch")]
        public Switch Switch { get; set; }

		public SettingsCell()
		{
			Size = new Size() { Height = 50 };
			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};
			Value = new Label
			{
                TextColor = (Color)Theme.ColorPalette.TextGray.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};
            Switch = (Switch)Theme.ControlPalette.Switch.Clone();
		}
	}

    public class Regions 
    {
		[JsonProperty("cell")]
		public RegionCell RegionCell { get; private set; }

		[JsonProperty("headerCell")]
		public RegionHeaderCell RegionHeaderCell { get; private set; }

        [JsonProperty("searchBar")]
        public SearchBar SearchBar { get; private set; }

        public Regions ()
        {
            RegionCell = new RegionCell();
            RegionHeaderCell = new RegionHeaderCell();
            SearchBar = (SearchBar)Theme.ControlPalette.SearchBar.Clone();
        }
    }

    public class RegionCell : View
	{
		[JsonProperty("size")]
		public Size Size { get; set; }

		[JsonProperty("title")]
		public Label Title { get; set; }

		public RegionCell()
		{
			Size = new Size() { Height = 50 };
			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
				Font = Theme.FontsPalette.SemiboldOfSize(14)
			};
		}
	}

    public class RegionHeaderCell : View
	{
		[JsonProperty("size")]
		public Size Size { get; set; }

		[JsonProperty("title")]
		public Label Title { get; set; }

		public RegionHeaderCell()
		{
			Size = new Size() { Height = 50 };
			Title = new Label
			{
				TextColor = (Color)Theme.ColorPalette.TextBase.Clone(),
                Font = Theme.FontsPalette.BoldOfSize(16)
			};
		}
	}
}
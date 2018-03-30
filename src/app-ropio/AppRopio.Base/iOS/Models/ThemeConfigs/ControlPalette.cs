using System;
using Newtonsoft.Json;

namespace AppRopio.Base.iOS.Models.ThemeConfigs
{
    public class ControlPalette
	{
		[JsonProperty("navigationBar")]
		public NavigationBar NavigationBar { get; set; }

		[JsonProperty("searchBar")]
		public SearchBar SearchBar { get; set; }

		[JsonProperty("textField")]
		public TextField TextField { get; set; }

        [JsonProperty("button")]
        public ButtonsPalette Button { get; set; }

		[JsonProperty("switch")]
		public Switch Switch { get; set; }

        [JsonProperty("notifications")]
        public UserNotifications Notifications { get; set; }

        [JsonProperty("inputNavigation")]
        public InputNavigationView InputNavigation { get; set; }

        [JsonProperty("cell")]
        public CellsPalette Cell { get; set; }
	}
}

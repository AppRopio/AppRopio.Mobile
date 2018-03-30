using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Base.Settings.Core.Models.Bundle
{
    public class SettingsPickerBundle : BaseBundle
    {
        public string Id { get; private set; }

		public string Title { get; private set; }

		public SettingsPickerBundle()
		{

		}

        public SettingsPickerBundle(NavigationType navigationType, string selectedRegionId, string selectedRegionTitle)
			 : base(navigationType, new Dictionary<string, string>
			{
            { nameof(Id), selectedRegionId },
			{ nameof(Title), selectedRegionTitle },
			})
		{
		}
	}
}
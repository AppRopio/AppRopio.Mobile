using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.Settings.Core.Models;
using MvvmCross;

namespace AppRopio.Base.Settings.Core.Services.Implementation
{
    public class SettingsConfigService : ISettingsConfigService
    {
		#region Properties

		private SettingsConfig _config;
		public SettingsConfig Config
		{
			get
			{
				return _config ?? (_config = LoadConfigFromJSON());
			}
		}

		#endregion

		#region Private

		private SettingsConfig LoadConfigFromJSON()
		{
			var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, SettingsConstants.CONFIG_NAME);
			var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsConfig>(json);
		}

		#endregion
	}
}
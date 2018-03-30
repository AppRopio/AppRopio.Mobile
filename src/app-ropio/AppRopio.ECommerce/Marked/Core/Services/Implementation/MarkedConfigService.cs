using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.ECommerce.Marked.Core.Models;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Marked.Core.Services.Implementation
{
public class MarkedConfigService : IMarkedConfigService
	{
		#region Properties

		private MarkedConfig _config;
		public MarkedConfig Config
		{
			get
			{
				return _config ?? (_config = LoadConfigFromJSON());
			}
		}

		#endregion

		#region Private

		private MarkedConfig LoadConfigFromJSON()
		{
			var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, MarkedConstants.CONFIG_NAME);
			var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<MarkedConfig>(json);
		}

		#endregion
	}
}

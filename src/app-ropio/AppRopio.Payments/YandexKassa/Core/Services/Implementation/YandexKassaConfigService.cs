using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Payments.YandexKassa.Core.Models;
using MvvmCross.Platform;

namespace AppRopio.Payments.YandexKassa.Core.Services.Implementation
{
    public class YandexKassaConfigService : IYandexKassaConfigService
    {
		#region Properties

		private YandexKassaConfig _config;
		public YandexKassaConfig Config
		{
			get
			{
				return _config ?? (_config = LoadConfigFromJSON());
			}
		}

		#endregion

		#region Private

		private YandexKassaConfig LoadConfigFromJSON()
		{
			var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, YandexKassaConstants.CONFIG_NAME);
			var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<YandexKassaConfig>(json);
		}

		#endregion
	}
}
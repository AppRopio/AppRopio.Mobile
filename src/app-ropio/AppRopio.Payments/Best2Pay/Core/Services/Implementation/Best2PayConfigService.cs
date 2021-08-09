using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Payments.Best2Pay.Core.Models;
using MvvmCross;

namespace AppRopio.Payments.Best2Pay.Core.Services.Implementation
{
    public class Best2PayConfigService : IBest2PayConfigService
    {
		#region Properties

		private Best2PayConfig _config;
		public Best2PayConfig Config
		{
			get
			{
				return _config ?? (_config = LoadConfigFromJSON());
			}
		}

		#endregion

		#region Private

		private Best2PayConfig LoadConfigFromJSON()
		{
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, Best2PayConstants.CONFIG_NAME);
			var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Best2PayConfig>(json);
		}

		#endregion
	}
}
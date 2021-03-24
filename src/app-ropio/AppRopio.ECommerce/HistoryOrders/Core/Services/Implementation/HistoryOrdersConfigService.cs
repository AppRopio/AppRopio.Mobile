using System;
using System.IO;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.ECommerce.HistoryOrders.Core.Models;
using MvvmCross;

namespace AppRopio.ECommerce.HistoryOrders.Core.Services.Implementation
{
    public class HistoryOrdersConfigService : IHistoryOrdersConfigService
	{
		#region Properties

		private HistoryOrdersConfig _config;
		public HistoryOrdersConfig Config
		{
			get
			{
				return _config ?? (_config = LoadConfigFromJSON());
			}
		}

		#endregion

		#region Private

		private HistoryOrdersConfig LoadConfigFromJSON()
		{
			var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, HistoryOrdersConstants.CONFIG_NAME);
			var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<HistoryOrdersConfig>(json);
		}

		#endregion
	}
}

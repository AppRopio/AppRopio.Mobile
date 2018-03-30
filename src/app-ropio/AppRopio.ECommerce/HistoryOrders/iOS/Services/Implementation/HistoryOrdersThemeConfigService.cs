using System;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.ECommerce.HistoryOrders.Core;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Services.Implementation
{
	public class HistoryOrdersThemeConfigService : BaseThemeConfigService<HistoryOrdersThemeConfig>, 
                                                   IHistoryOrdersThemeConfigService
	{
		protected override string ConfigName
		{
			get
			{
				return HistoryOrdersConstants.CONFIG_NAME;
			}
		}
	}
}

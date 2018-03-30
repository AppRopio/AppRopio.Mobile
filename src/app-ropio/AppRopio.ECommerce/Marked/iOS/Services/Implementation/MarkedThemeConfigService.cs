using System;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.ECommerce.Marked.Core;
using AppRopio.ECommerce.Marked.iOS.Models;

namespace AppRopio.ECommerce.Marked.iOS.Services.Implementation
{
	public class MarkedThemeConfigService : BaseThemeConfigService<MarkedThemeConfig>, IMarkedThemeConfigService
	{
        protected override string ConfigName
		{
			get
			{
				return MarkedConstants.CONFIG_NAME;
			}
		}
	}
}
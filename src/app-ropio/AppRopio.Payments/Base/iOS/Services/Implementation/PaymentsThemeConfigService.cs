using System;
using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.Payments.Core;
using AppRopio.Payments.iOS.Models;

namespace AppRopio.Payments.iOS.Services.Implementation
{
	public class PaymentsThemeConfigService : BaseThemeConfigService<PaymentsThemeConfig>, IPaymentsThemeConfigService
	{
		protected override string ConfigName
		{
			get
			{
				return PaymentsConstants.CONFIG_NAME;
			}
		}
	}
}
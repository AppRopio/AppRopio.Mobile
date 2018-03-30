using AppRopio.Base.Auth.Core;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.iOS.Services.ThemeConfig;

namespace AppRopio.Base.Auth.iOS.Services.Implementation
{
	public class AuthThemeConfigService: BaseThemeConfigService<AuthThemeConfig>, IAuthThemeConfigService
	{
		protected override string ConfigName
		{
			get
			{
				return AuthConst.CONFIG_NAME;
			}
		}
	}
}

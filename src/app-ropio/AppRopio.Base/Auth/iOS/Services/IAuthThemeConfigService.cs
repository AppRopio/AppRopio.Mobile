using System;
using AppRopio.Base.Auth.iOS.Models;

namespace AppRopio.Base.Auth.iOS.Services
{
	public interface IAuthThemeConfigService
	{
		AuthThemeConfig ThemeConfig { get; }
	}
}

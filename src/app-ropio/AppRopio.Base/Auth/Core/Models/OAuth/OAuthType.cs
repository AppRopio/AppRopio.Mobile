using AppRopio.Models.Auth.Enums;

namespace AppRopio.Base.Auth.Core.Models.OAuth
{
	/// <summary>
	/// Типы возможных авторизаций через протокол OAuth2
	/// </summary>
	public enum OAuthType
	{
		Unknown = 0,
		Facebook,
		/// <summary>
		/// Не поддерживается сейчас
		/// </summary>
		Twitter,
		VK,
		OK,
		/// <summary>
		/// Не поддерживается сейчас
		/// </summary>
		GPlus
	}
}
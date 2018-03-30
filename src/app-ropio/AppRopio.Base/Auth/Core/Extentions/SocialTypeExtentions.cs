using System;
using AppRopio.Base.Auth.Core.Models.OAuth;
using AppRopio.Models.Auth.Enums;

namespace AppRopio.Base.Auth.Core.Extentions
{
	public static class SocialTypeExtentions
	{
		public static SocialType GetSocialType(this OAuthType oauthType)
		{
			switch (oauthType)
			{
				case OAuthType.Facebook:
					return SocialType.Facebook;
				case OAuthType.GPlus:
					return SocialType.GPlus;
				case OAuthType.OK:
					return SocialType.OK;
				case OAuthType.Twitter:
					return SocialType.Twitter;
				case OAuthType.VK:
					return SocialType.VK;
			}
			return SocialType.Unknown;
		}

		public static OAuthType GetOAuthType(this SocialType socialType)
		{
			switch (socialType)
			{
				case SocialType.Facebook:
					return OAuthType.Facebook;
				case SocialType.GPlus:
					return OAuthType.GPlus;
				case SocialType.OK:
					return OAuthType.OK;
				case SocialType.Twitter:
					return OAuthType.Twitter;
				case SocialType.VK:
					return OAuthType.VK;
			}
			return OAuthType.Unknown;
		}

	}

}

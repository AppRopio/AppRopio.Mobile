using System;
using System.Collections.Generic;
using AppRopio.Base.Auth.Core.Models.Registration;

namespace AppRopio.Base.Auth.Core.Models.Config
{
    public class AuthConfig
    {
        public bool SocialButtonsEnable { get; set; }
		//вход по телефону или по email
		public bool IdentifyUserByEmail { get; set; }

		public string LegalText { get; set; }

		public string LegalUrl { get; set; }

		public bool RequireConfirmPassword { get; set; }

		public List<RegistrationField> Items { get; set; }
    }
}

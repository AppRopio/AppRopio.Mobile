using System.IO;
using AppRopio.Base.Auth.Core.Models.Config;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross;

namespace AppRopio.Base.Auth.Core.Services.Implementation
{
	public class AuthConfigService : IAuthConfigService
    {
        #region Fields

        protected virtual string SettingsName { get { return "Auth.json"; } }

        #endregion

        #region Properties

        private AuthConfig _config;
        public AuthConfig Config
        {
            get
            {
				//_config ?? (_config = LoadConfigFromJSON());
				if (_config==null)
				{
					_config = LoadConfigFromJSON();
				}
				return _config;
            }
        }

        #endregion

        #region Private

        private AuthConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, SettingsName);
            var json = Mvx.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<AuthConfig>(json);
        }

        #endregion
    }
}

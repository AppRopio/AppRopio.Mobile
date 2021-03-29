using System;
using System.IO;
using AppRopio.Base.Contacts.Core.Models;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Settings;
using MvvmCross;

namespace AppRopio.Base.Contacts.Core.Services.Implementation
{
    public class ContactsConfigService : IContactsConfigService
    {
        #region Properties

        private ContactsConfig _config;
        public ContactsConfig Config
        {
            get
            {
                return _config ?? (_config = LoadConfigFromJSON());
            }
        }

        #endregion

        #region Private

        private ContactsConfig LoadConfigFromJSON()
        {
            var path = Path.Combine(CoreConstants.CONFIGS_FOLDER, ContactsConstants.CONFIG_NAME);
            var json = Mvx.IoCProvider.Resolve<ISettingsService>().ReadStringFromFile(path);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ContactsConfig>(json);
        }

        #endregion
    }
}

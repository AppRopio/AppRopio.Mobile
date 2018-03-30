using AppRopio.Base.Contacts.Core;
using AppRopio.Base.Contacts.iOS.Models;
using AppRopio.Base.iOS.Services.ThemeConfig;

namespace AppRopio.Base.Contacts.iOS.Services.Implementation
{
    public class ContactsThemeConfigService : BaseThemeConfigService<ContactsThemeConfig>, IContactsThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return ContactsConstants.CONFIG_NAME;
            }
        }
    }
}

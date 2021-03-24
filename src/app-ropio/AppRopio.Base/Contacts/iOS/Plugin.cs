using AppRopio.Base.Contacts.Core;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using AppRopio.Base.Contacts.iOS.Services;
using AppRopio.Base.Contacts.iOS.Services.Implementation;
using AppRopio.Base.Contacts.iOS.Views.Contacts;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Contacts.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IContactsThemeConfigService>(() => new ContactsThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IContactsViewModel, ContactsViewController>();


        }
    }
}

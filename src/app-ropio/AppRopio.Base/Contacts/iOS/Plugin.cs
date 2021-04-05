using AppRopio.Base.Contacts.Core;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using AppRopio.Base.Contacts.iOS.Services;
using AppRopio.Base.Contacts.iOS.Services.Implementation;
using AppRopio.Base.Contacts.iOS.Views.Contacts;
using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Contacts.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Contacts";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IContactsThemeConfigService>(() => new ContactsThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IContactsViewModel, ContactsViewController>();
        }
    }
}

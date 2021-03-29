using AppRopio.Base.API;
using AppRopio.Base.Contacts.API.Services;
using AppRopio.Base.Contacts.API.Services.Implementation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Contacts.API
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IContactsService>(() => new FakeContactsService());
            else
                Mvx.IoCProvider.RegisterType<IContactsService>(() => new ContactsService());
        }
    }
}

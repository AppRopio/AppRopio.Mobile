using AppRopio.Base.API;
using AppRopio.Base.Contacts.API.Services;
using AppRopio.Base.Contacts.API.Services.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Base.Contacts.API
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            //if (ApiSettings.DebugServiceEnabled)
            //    Mvx.RegisterType<IContactsService>(() => new FakeContactsService());
            //else
                Mvx.RegisterType<IContactsService>(() => new ContactsService());
        }
    }
}

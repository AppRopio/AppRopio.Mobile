using AppRopio.Base.Contacts.Core.Services;
using AppRopio.Base.Contacts.Core.Services.Implementation;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts.Services;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Contacts.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterType<IContactsNavigationVmService>(() => new ContactsNavigationVmService());

            Mvx.RegisterSingleton<IContactsConfigService>(() => new ContactsConfigService());

            Mvx.RegisterSingleton<IContactsVmService>(() => new ContactsVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

            vmLookupService.Register<IContactsViewModel>(typeof(ContactsViewModel));

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.Resolve<IRouterService>();

            routerService.Register<IContactsViewModel>(new ContactsRouterSubscriber());

            #endregion
        }
    }
}

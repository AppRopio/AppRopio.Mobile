using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Contacts.API.Services;
using AppRopio.Base.Core.Services.Launcher;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.Location;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Contacts.Enums;
using AppRopio.Models.Contacts.Responses;
using MvvmCross;

namespace AppRopio.Base.Contacts.Core.ViewModels.Contacts.Services
{
    public class ContactsVmService : BaseVmService, IContactsVmService
    {
        #region Services

        protected IContactsService ApiService => Mvx.IoCProvider.Resolve<IContactsService>();

        protected ILocationService LocationService => Mvx.IoCProvider.Resolve<ILocationService>();

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        #endregion

        public async Task<List<ListResponseItem>> LoadContacts()
        {
            List<ListResponseItem> dataSource = null;

            try
            {
                var position = await LocationService.GetCurrentLocation();

                var contacts = await ApiService.LoadContacts(position);

                dataSource = contacts.Contacts;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        public async Task HandleContactSelection(ListResponseItem contact)
        {
            var launcher = Mvx.IoCProvider.Resolve<ILauncherService>();
            var userDialogs = Mvx.IoCProvider.Resolve<IUserDialogs>();

            switch (contact.Type)
            {
                case ContactTypes.Email:
                    await launcher.LaunchEmail(contact.Value);
                    break;

                case ContactTypes.Phone:
                    if (await userDialogs.Confirm($"{LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "CallTo")} {contact.DisplayValue}?", LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "MakeCall")))
                        await launcher.LaunchPhone(contact.DisplayValue);
                    break;

                case ContactTypes.Url:
                    await launcher.LaunchUri(contact.Value);
                    break;

                case ContactTypes.Address:
                    if (await userDialogs.Confirm(LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "OpenInMap"), LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "GoToMap")))
                        await launcher.LaunchAddress(contact.Value);
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.Models.Contacts;
using AppRopio.Base.Core.Services.Contacts;
using ContactsUI;
using MvvmCross.Platform;
using System.Linq;
using UIKit;
using MvvmCross.Platform.Core;
using Foundation;
using Contacts;
using Plugin.Permissions.Abstractions;
using AppRopio.Base.Core.Services.Permissions;

namespace AppRopio.Base.iOS.Services.Contacts
{
    public class ContactsService : IContactsService
    {
        private readonly string _permissionMessage = "Доступ к контактам запрещен, необходимо изменить настройки приложения";
        protected IPermissionsService PermissionsService => Mvx.Resolve<IPermissionsService>();

        protected UIViewController GetPresentedViewController()
        {
            var window= UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            return vc;
        }

        #region IContactsService implementation

        public Task<List<Contact>> GetContacts()
        {
            throw new NotImplementedException();
        }

        public Task<Contact> SelectContact()
        {
            var tcs = new TaskCompletionSource<Contact>();

            Task.Run(async () =>
            {
                var hasPermission = await PermissionsService.CheckPermission(Permission.Contacts, true, _permissionMessage);
                if (!hasPermission)
                {
                    tcs.TrySetCanceled();
                    return;
                }

                var picker = new CNContactPickerViewController();

                var pickerDelegate = new ContactPickerDelegate();
                picker.Delegate = pickerDelegate;

                pickerDelegate.SelectionCanceled = () => tcs.TrySetCanceled();
                pickerDelegate.ContactPropertySelected = (prop) => pickerDelegate.ContactSelected?.Invoke(prop.Contact);
                pickerDelegate.ContactSelected = (contact) =>
                {
                    tcs.TrySetResult(new Contact
                    {
                        FirstName = contact.GivenName,
                        LastName = contact.FamilyName,
                        MiddleName = contact.MiddleName,
                        Phones = contact.PhoneNumbers?.Select(x => new Phone { FullValue = x.Value.StringValue }).ToList(),
                        Emails = contact.EmailAddresses?.Select(x => new Email { FullValue = x.Value }).ToList()
                    });
                };

                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                {
                    GetPresentedViewController().PresentViewController(picker, true, null);
                });
            });

            return tcs.Task;
        }

        public Task<Phone> SelectPhone()
        {
            var tcs = new TaskCompletionSource<Phone>();

            Task.Run(async () =>
            {
                var hasPermission = await PermissionsService.CheckPermission(Permission.Contacts, true, _permissionMessage);
                if (!hasPermission)
                {
                    tcs.TrySetCanceled();
                    return;
                }

                var picker = new CNContactPickerViewController
                {
                    DisplayedPropertyKeys = new NSString[] { CNContactKey.PhoneNumbers },
                    PredicateForEnablingContact = NSPredicate.FromFormat("phoneNumbers.@count > 0"),
                    PredicateForSelectionOfContact = NSPredicate.FromFormat("phoneNumbers.@count == 1")
                };

                var pickerDelegate = new ContactPickerDelegate();
                picker.Delegate = pickerDelegate;

                pickerDelegate.SelectionCanceled = () => tcs.TrySetCanceled();
                pickerDelegate.ContactPropertySelected = (prop) =>
                {
                    var phone = (prop?.Value as CNPhoneNumber)?.StringValue;
                    tcs.TrySetResult((prop == null) ? null : new Phone { FullValue = phone });
                };
                pickerDelegate.ContactSelected = (contact) =>
                {
                    var phone = contact.PhoneNumbers?.FirstOrDefault()?.Value?.StringValue;
                    tcs.TrySetResult((phone == null) ? null : new Phone { FullValue = phone });
                };

                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                {
                    GetPresentedViewController().PresentViewController(picker, true, null);
                });
            });

            return tcs.Task;
        }

        public Task<Email> SelectEmail()
        {
            var tcs = new TaskCompletionSource<Email>();

            Task.Run(async () =>
            {
                var hasPermission = await PermissionsService.CheckPermission(Permission.Contacts, true, _permissionMessage);
                if (!hasPermission)
                {
                    tcs.TrySetCanceled();
                    return;
                }

                var picker = new CNContactPickerViewController
                {
                    DisplayedPropertyKeys = new NSString[] { CNContactKey.EmailAddresses },
                    PredicateForEnablingContact = NSPredicate.FromFormat("emailAddresses.@count > 0"),
                    PredicateForSelectionOfContact = NSPredicate.FromFormat("emailAddresses.@count == 1")
                };

                var pickerDelegate = new ContactPickerDelegate();
                picker.Delegate = pickerDelegate;

                pickerDelegate.SelectionCanceled = () => tcs.TrySetCanceled();
                pickerDelegate.ContactPropertySelected = (prop) =>
                {
                    var email = prop?.Value as NSString;
                    tcs.TrySetResult((email == null) ? null : new Email { FullValue = email });
                };
                pickerDelegate.ContactSelected = (contact) =>
                {
                    var email = contact.EmailAddresses?.FirstOrDefault()?.Value;
                    tcs.TrySetResult((email == null) ? null : new Email { FullValue = email });
                };

                Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                {
                    GetPresentedViewController().PresentViewController(picker, true, null);
                });
            });

            return tcs.Task;
        }

        #endregion
    }
}

using System;
using Contacts;
using ContactsUI;
namespace AppRopio.Base.iOS.Services.Contacts
{
    public class ContactPickerDelegate : CNContactPickerDelegate
    {
        public Action SelectionCanceled { get; set; }
        public Action<CNContact> ContactSelected { get; set; }
        public Action<CNContactProperty> ContactPropertySelected { get; set; }

        #region Constructors

        public ContactPickerDelegate()
        {
        }

        public ContactPickerDelegate(IntPtr handle) : base (handle)
        {
        }

        #endregion

        public override void ContactPickerDidCancel(CNContactPickerViewController picker)
        {
            SelectionCanceled?.Invoke();
        }

        public override void DidSelectContact(CNContactPickerViewController picker, CNContact contact)
        {
            ContactSelected?.Invoke(contact);
        }

        public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty)
        {
            ContactPropertySelected?.Invoke(contactProperty);
        }
    }
}

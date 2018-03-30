// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Contacts.iOS.Views.Contacts
{
    [Register ("ContactsViewController")]
    partial class ContactsViewController
    {
        [Outlet]
        UIKit.UITableView contactsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint _tableViewBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_tableViewBottomConstraint != null) {
                _tableViewBottomConstraint.Dispose ();
                _tableViewBottomConstraint = null;
            }

            if (contactsTableView != null) {
                contactsTableView.Dispose ();
                contactsTableView = null;
            }
        }
    }
}
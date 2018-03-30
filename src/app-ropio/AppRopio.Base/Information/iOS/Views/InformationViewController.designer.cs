// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Information.iOS.Views
{
    [Register ("InformationViewController")]
    partial class InformationViewController
    {
        [Outlet]
        UIKit.UITableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint _tableViewBottomConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_tableViewBottomConstraint != null) {
                _tableViewBottomConstraint.Dispose ();
                _tableViewBottomConstraint = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}
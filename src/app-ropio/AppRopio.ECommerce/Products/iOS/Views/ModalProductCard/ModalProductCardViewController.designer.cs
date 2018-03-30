// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard
{
    [Register ("ModalProductCardViewController")]
    partial class ModalProductCardViewController
    {
        [Outlet]
        UIKit.UIButton _closeButton { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _closeButtonTopConstraint { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_closeButton != null) {
                _closeButton.Dispose ();
                _closeButton = null;
            }

            if (_closeButtonTopConstraint != null) {
                _closeButtonTopConstraint.Dispose ();
                _closeButtonTopConstraint = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }
        }
    }
}
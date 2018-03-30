// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Payment
{
    [Register("PaymentViewController")]
    partial class PaymentViewController
    {
        [Outlet]
        UIKit.UIButton _cancelBtn { get; set; }

        [Outlet]
        UIKit.UIView _containerView { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint _containerViewHeightConstraint { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint _containerViewTopConstraint { get; set; }

        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

        [Outlet]
        UIKit.UIView _titleSeparator { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_titleSeparator != null) {
                _titleSeparator.Dispose ();
                _titleSeparator = null;
            }

            if (_cancelBtn != null) {
                _cancelBtn.Dispose ();
                _cancelBtn = null;
            }

            if (_containerView != null) {
                _containerView.Dispose ();
                _containerView = null;
            }

            if (_containerViewHeightConstraint != null) {
                _containerViewHeightConstraint.Dispose ();
                _containerViewHeightConstraint = null;
            }

            if (_containerViewTopConstraint != null) {
                _containerViewTopConstraint.Dispose ();
                _containerViewTopConstraint = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}

// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.SignUp
{
    [Register ("SignUpViewController")]
    partial class SignUpViewController
    {
        [Outlet]
        UIKit.UIImageView _iconImage { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageWidth { get; set; }


        [Outlet]
        UIKit.UITextView _legalTextView { get; set; }


        [Outlet]
        UIKit.UIButton _nextButton { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_iconImage != null) {
                _iconImage.Dispose ();
                _iconImage = null;
            }

            if (_imageHeight != null) {
                _imageHeight.Dispose ();
                _imageHeight = null;
            }

            if (_imageWidth != null) {
                _imageWidth.Dispose ();
                _imageWidth = null;
            }

            if (_legalTextView != null) {
                _legalTextView.Dispose ();
                _legalTextView = null;
            }

            if (_nextButton != null) {
                _nextButton.Dispose ();
                _nextButton = null;
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
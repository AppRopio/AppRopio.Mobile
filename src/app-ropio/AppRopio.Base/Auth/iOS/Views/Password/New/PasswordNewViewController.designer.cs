// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.Password.New
{
    [Register ("PasswordNewViewController")]
    partial class PasswordNewViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint _imageHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageWidth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView _bottomView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel _descriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton _doneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView _headerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView _iconImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARTextField _passwordConfirmField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARTextField _passwordField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_bottomView != null) {
                _bottomView.Dispose ();
                _bottomView = null;
            }

            if (_descriptionLabel != null) {
                _descriptionLabel.Dispose ();
                _descriptionLabel = null;
            }

            if (_doneButton != null) {
                _doneButton.Dispose ();
                _doneButton = null;
            }

            if (_headerView != null) {
                _headerView.Dispose ();
                _headerView = null;
            }

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

            if (_passwordConfirmField != null) {
                _passwordConfirmField.Dispose ();
                _passwordConfirmField = null;
            }

            if (_passwordField != null) {
                _passwordField.Dispose ();
                _passwordField = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.Password.Reset.Sms
{
    [Register ("ResetSmsViewController")]
    partial class ResetSmsViewController
    {
        [Outlet]
        UIKit.UIView _bottomView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView _iconImage { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageWidth { get; set; }


        [Outlet]
        UIKit.UIButton _resendCodeButton { get; set; }


        [Outlet]
        UIKit.UIView _spacingView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }


        [Outlet]
        UIKit.UIButton _validateCodeButton { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARTextField _verificationCodeField { get; set; }

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

            if (_resendCodeButton != null) {
                _resendCodeButton.Dispose ();
                _resendCodeButton = null;
            }

            if (_spacingView != null) {
                _spacingView.Dispose ();
                _spacingView = null;
            }

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }

            if (_validateCodeButton != null) {
                _validateCodeButton.Dispose ();
                _validateCodeButton = null;
            }

            if (_verificationCodeField != null) {
                _verificationCodeField.Dispose ();
                _verificationCodeField = null;
            }
        }
    }
}
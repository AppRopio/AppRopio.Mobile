// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.Thanks
{
    [Register ("ThanksViewController")]
    partial class ThanksViewController
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIButton _doneButton { get; set; }


        [Outlet]
        UIKit.UIImageView _iconImage { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _imageWidth { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_descriptionLabel != null) {
                _descriptionLabel.Dispose ();
                _descriptionLabel = null;
            }

            if (_doneButton != null) {
                _doneButton.Dispose ();
                _doneButton = null;
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

            if (_titleLabel != null) {
                _titleLabel.Dispose ();
                _titleLabel = null;
            }
        }
    }
}
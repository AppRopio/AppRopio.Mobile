// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.Auth
{
    [Register ("AuthViewController")]
    partial class AuthViewController
    {
        [Outlet]
        UIKit.UIButton _facebookBtn { get; set; }


        [Outlet]
        UIKit.UIImageView _logoImg { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _logoImgHeight { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _logoImgWidth { get; set; }


        [Outlet]
        UIKit.UIButton _signInBtn { get; set; }


        [Outlet]
        UIKit.UIButton _signUpBtn { get; set; }


        [Outlet]
        UIKit.UIButton _skipBtn { get; set; }


        [Outlet]
        UIKit.UIButton _vkBtn { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_facebookBtn != null) {
                _facebookBtn.Dispose ();
                _facebookBtn = null;
            }

            if (_logoImg != null) {
                _logoImg.Dispose ();
                _logoImg = null;
            }

            if (_logoImgHeight != null) {
                _logoImgHeight.Dispose ();
                _logoImgHeight = null;
            }

            if (_logoImgWidth != null) {
                _logoImgWidth.Dispose ();
                _logoImgWidth = null;
            }

            if (_signInBtn != null) {
                _signInBtn.Dispose ();
                _signInBtn = null;
            }

            if (_signUpBtn != null) {
                _signUpBtn.Dispose ();
                _signUpBtn = null;
            }

            if (_skipBtn != null) {
                _skipBtn.Dispose ();
                _skipBtn = null;
            }

            if (_vkBtn != null) {
                _vkBtn.Dispose ();
                _vkBtn = null;
            }
        }
    }
}
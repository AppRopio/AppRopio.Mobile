// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Transition
{
    [Register ("TransitionCell")]
    partial class TransitionCell
    {
        [Outlet]
        UIKit.UIImageView _accessoryImageView { get; set; }


        [Outlet]
        UIKit.UIView _bottomSeparator { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _value { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_accessoryImageView != null) {
                _accessoryImageView.Dispose ();
                _accessoryImageView = null;
            }

            if (_bottomSeparator != null) {
                _bottomSeparator.Dispose ();
                _bottomSeparator = null;
            }

            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }

            if (_value != null) {
                _value.Dispose ();
                _value = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Cells
{
    [Register ("PDHorizontalTextCell")]
    partial class PDHorizontalTextCell
    {
        [Outlet]
        UIKit.UIView _colorView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _valueName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_colorView != null) {
                _colorView.Dispose ();
                _colorView = null;
            }

            if (_valueName != null) {
                _valueName.Dispose ();
                _valueName = null;
            }
        }
    }
}
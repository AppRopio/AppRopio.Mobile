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
    [Register ("PDHorizontalColorCell")]
    partial class PDHorizontalColorCell
    {
        [Outlet]
        UIKit.UIView _colorView { get; set; }

        [Outlet]
        UIKit.UIView _selectedView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_colorView != null) {
                _colorView.Dispose ();
                _colorView = null;
            }

            if (_selectedView != null) {
                _selectedView.Dispose ();
                _selectedView = null;
            }
        }
    }
}
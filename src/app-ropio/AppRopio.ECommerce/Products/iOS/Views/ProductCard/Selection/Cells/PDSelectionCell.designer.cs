// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Selection.Cells
{
    [Register ("PDSelectionCell")]
    partial class PDSelectionCell
    {
        [Outlet]
        UIKit.UIView _bottomSeparator { get; set; }

        [Outlet]
        UIKit.UIImageView _selectionImageView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _valueName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_bottomSeparator != null) {
                _bottomSeparator.Dispose ();
                _bottomSeparator = null;
            }

            if (_selectionImageView != null) {
                _selectionImageView.Dispose ();
                _selectionImageView = null;
            }

            if (_valueName != null) {
                _valueName.Dispose ();
                _valueName = null;
            }
        }
    }
}
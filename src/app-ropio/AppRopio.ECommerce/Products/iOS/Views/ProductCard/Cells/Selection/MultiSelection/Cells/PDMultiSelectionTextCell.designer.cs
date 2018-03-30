// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection.Cells
{
    [Register ("PDMultiSelectionTextCell")]
    partial class PDMultiSelectionTextCell
    {
        [Outlet]
        UIKit.UIImageView _crossImageView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        [Outlet]
        UIKit.UIView _selectedView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_crossImageView != null) {
                _crossImageView.Dispose ();
                _crossImageView = null;
            }

            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }

            if (_selectedView != null) {
                _selectedView.Dispose ();
                _selectedView = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells
{
    [Register ("CatalogGridCell")]
    partial class CatalogGridCell
    {
        [Outlet]
        UIKit.UICollectionView _badges { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _badgesWidthContraint { get; set; }


        [Outlet]
        UIKit.UIImageView _image { get; set; }


        [Outlet]
        UIKit.UIButton _markButton { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _oldPrice { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _price { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_badges != null) {
                _badges.Dispose ();
                _badges = null;
            }

            if (_badgesWidthContraint != null) {
                _badgesWidthContraint.Dispose ();
                _badgesWidthContraint = null;
            }

            if (_image != null) {
                _image.Dispose ();
                _image = null;
            }

            if (_markButton != null) {
                _markButton.Dispose ();
                _markButton = null;
            }

            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }

            if (_oldPrice != null) {
                _oldPrice.Dispose ();
                _oldPrice = null;
            }

            if (_price != null) {
                _price.Dispose ();
                _price = null;
            }
        }
    }
}
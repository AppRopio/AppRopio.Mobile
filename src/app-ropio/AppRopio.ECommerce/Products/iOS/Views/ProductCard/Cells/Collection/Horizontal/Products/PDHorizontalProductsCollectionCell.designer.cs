// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products
{
    [Register ("PDHorizontalProductsCollectionCell")]
    partial class PDHorizontalProductsCollectionCell
    {
        [Outlet]
        UIKit.UIView _bottomSeparator { get; set; }


        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _collectionViewHeightConstraint { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_bottomSeparator != null) {
                _bottomSeparator.Dispose ();
                _bottomSeparator = null;
            }

            if (_collectionView != null) {
                _collectionView.Dispose ();
                _collectionView = null;
            }

            if (_collectionViewHeightConstraint != null) {
                _collectionViewHeightConstraint.Dispose ();
                _collectionViewHeightConstraint = null;
            }

            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }
        }
    }
}
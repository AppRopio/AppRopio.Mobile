// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.Collection.Horizontal.Products
{
    [Register("ModalHorizontalProductsCollectionCell")]
    partial class ModalHorizontalProductsCollectionCell
    {
        [Outlet]
        UIKit.UIView _bottomSeparator { get; set; }


        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint _collectionViewHeightConstraint { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_bottomSeparator != null)
            {
                _bottomSeparator.Dispose();
                _bottomSeparator = null;
            }

            if (_collectionView != null)
            {
                _collectionView.Dispose();
                _collectionView = null;
            }

            if (_collectionViewHeightConstraint != null)
            {
                _collectionViewHeightConstraint.Dispose();
                _collectionViewHeightConstraint = null;
            }

            if (_name != null)
            {
                _name.Dispose();
                _name = null;
            }
        }
    }
}

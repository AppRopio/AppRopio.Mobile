// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Disabled
{
    [Register ("DCategoriesViewController")]
    partial class DCategoriesViewController
    {
        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint _collectionViewBottomConstraint { get; set; }

        [Outlet]
        UIKit.NSLayoutConstraint _collectionViewTopConstraint { get; set; }

        [Outlet]
        UIKit.UIImageView _emptyImage { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _emptyText { get; set; }

        [Outlet]
        UIKit.UIView _emptyView { get; set; }

        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _epmtyTitle { get; set; }

        [Outlet]
        UIKit.UIButton _goToButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_collectionView != null) {
                _collectionView.Dispose ();
                _collectionView = null;
            }

            if (_collectionViewBottomConstraint != null) {
                _collectionViewBottomConstraint.Dispose ();
                _collectionViewBottomConstraint = null;
            }

            if (_collectionViewTopConstraint != null) {
                _collectionViewTopConstraint.Dispose ();
                _collectionViewTopConstraint = null;
            }

            if (_emptyImage != null) {
                _emptyImage.Dispose ();
                _emptyImage = null;
            }

            if (_emptyText != null) {
                _emptyText.Dispose ();
                _emptyText = null;
            }

            if (_emptyView != null) {
                _emptyView.Dispose ();
                _emptyView = null;
            }

            if (_epmtyTitle != null) {
                _epmtyTitle.Dispose ();
                _epmtyTitle = null;
            }

            if (_goToButton != null) {
                _goToButton.Dispose ();
                _goToButton = null;
            }
        }
    }
}
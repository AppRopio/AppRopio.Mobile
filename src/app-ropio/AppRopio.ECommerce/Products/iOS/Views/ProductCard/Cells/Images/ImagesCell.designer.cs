// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images
{
    [Register ("ImagesCell")]
    partial class ImagesCell
    {
        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }


        [Outlet]
        UIKit.UIPageControl _pageControl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_collectionView != null) {
                _collectionView.Dispose ();
                _collectionView = null;
            }

            if (_pageControl != null) {
                _pageControl.Dispose ();
                _pageControl = null;
            }
        }
    }
}
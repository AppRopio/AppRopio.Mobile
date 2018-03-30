// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep
{
    [Register ("SSCategoriesViewController")]
    partial class SSCategoriesViewController
    {
        [Outlet]
        UIKit.UICollectionView _bottomCollection { get; set; }


        [Outlet]
        UIKit.UICollectionView _collectionView { get; set; }


        [Outlet]
        UIKit.UITableView _tableView { get; set; }


        [Outlet]
        UIKit.UICollectionView _topCollection { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_bottomCollection != null) {
                _bottomCollection.Dispose ();
                _bottomCollection = null;
            }

            if (_collectionView != null) {
                _collectionView.Dispose ();
                _collectionView = null;
            }

            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }

            if (_topCollection != null) {
                _topCollection.Dispose ();
                _topCollection = null;
            }
        }
    }
}
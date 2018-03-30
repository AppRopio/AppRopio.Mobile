// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch
{
    [Register ("ContentSearchViewController")]
    partial class ContentSearchViewController
    {
        [Outlet]
        UIKit.UICollectionView _autocompleteCollectionView { get; set; }


        [Outlet]
        UIKit.UIButton _clearHistoryBtn { get; set; }


        [Outlet]
        UIKit.UIView _hintsHeaderView { get; set; }


        [Outlet]
        UIKit.UITableView _hintsTableView { get; set; }


        [Outlet]
        UIKit.UIView _hintsView { get; set; }


        [Outlet]
        UIKit.UITableView _historyTableView { get; set; }


        [Outlet]
        UIKit.UIView _historyView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_autocompleteCollectionView != null) {
                _autocompleteCollectionView.Dispose ();
                _autocompleteCollectionView = null;
            }

            if (_clearHistoryBtn != null) {
                _clearHistoryBtn.Dispose ();
                _clearHistoryBtn = null;
            }

            if (_hintsHeaderView != null) {
                _hintsHeaderView.Dispose ();
                _hintsHeaderView = null;
            }

            if (_hintsTableView != null) {
                _hintsTableView.Dispose ();
                _hintsTableView = null;
            }

            if (_hintsView != null) {
                _hintsView.Dispose ();
                _hintsView = null;
            }

            if (_historyTableView != null) {
                _historyTableView.Dispose ();
                _historyTableView = null;
            }

            if (_historyView != null) {
                _historyView.Dispose ();
                _historyView = null;
            }
        }
    }
}
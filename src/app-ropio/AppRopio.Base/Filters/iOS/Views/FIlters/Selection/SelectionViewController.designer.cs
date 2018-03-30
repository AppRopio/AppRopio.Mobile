// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Selection
{
    [Register("SelectionViewController")]
    partial class SelectionViewController
    {
        [Outlet]
        UIKit.UIButton _applyBtn { get; set; }

        [Outlet]
        AppRopio.Base.iOS.BindableSearchBar _searchBar { get; set; }

        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (_applyBtn != null)
            {
                _applyBtn.Dispose();
                _applyBtn = null;
            }

            if (_searchBar != null)
            {
                _searchBar.Dispose();
                _searchBar = null;
            }

            if (_tableView != null)
            {
                _tableView.Dispose();
                _tableView = null;
            }
        }
    }
}

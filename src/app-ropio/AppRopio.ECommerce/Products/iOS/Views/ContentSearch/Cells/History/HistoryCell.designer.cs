// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.History
{
    [Register ("HistoryCell")]
    partial class HistoryCell
    {
        [Outlet]
        UIKit.UIImageView _historyImageView { get; set; }


        [Outlet]
        UIKit.UIImageView _linkImageView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _title { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_historyImageView != null) {
                _historyImageView.Dispose ();
                _historyImageView = null;
            }

            if (_linkImageView != null) {
                _linkImageView.Dispose ();
                _linkImageView = null;
            }

            if (_title != null) {
                _title.Dispose ();
                _title = null;
            }
        }
    }
}
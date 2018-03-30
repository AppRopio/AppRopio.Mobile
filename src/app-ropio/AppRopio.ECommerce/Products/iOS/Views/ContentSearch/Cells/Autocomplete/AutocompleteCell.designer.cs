// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Autocomplete
{
    [Register ("AutocompleteCell")]
    partial class AutocompleteCell
    {
        [Outlet]
        UIKit.UIView _backgroundView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _title { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_backgroundView != null) {
                _backgroundView.Dispose ();
                _backgroundView = null;
            }

            if (_title != null) {
                _title.Dispose ();
                _title = null;
            }
        }
    }
}
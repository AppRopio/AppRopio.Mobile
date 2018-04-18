// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Languages
{
    [Register("LanguagesViewController")]
    partial class LanguagesViewController
    {
        [Outlet]
        UIKit.UITableView TableView { get; set; }

        void ReleaseDesignerOutlets()
        {

            if (TableView != null)
            {
                TableView.Dispose();
                TableView = null;
            }
        }
    }
}

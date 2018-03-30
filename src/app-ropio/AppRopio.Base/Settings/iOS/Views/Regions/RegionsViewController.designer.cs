// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Settings.iOS.Views.Regions
{
    [Register ("RegionsViewController")]
    partial class RegionsViewController
    {
        [Outlet]
        UIKit.UITableView TableView { get; set; }

        void ReleaseDesignerOutlets ()
        {

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}
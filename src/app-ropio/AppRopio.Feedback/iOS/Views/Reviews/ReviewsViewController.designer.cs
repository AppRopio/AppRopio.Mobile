// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.Reviews
{
    [Register ("ReviewsViewController")]
    partial class ReviewsViewController
    {
        [Outlet]
        UIKit.UITableView TableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ReviewButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ReviewButton != null) {
                ReviewButton.Dispose ();
                ReviewButton = null;
            }

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.Reviews.Cell
{
    [Register ("ReviewCell")]
    partial class ReviewCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel AuthorLabel { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel BadgeLabel { get; set; }


        [Outlet]
        UIKit.UIView BadgeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel ContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel DateLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AuthorLabel != null) {
                AuthorLabel.Dispose ();
                AuthorLabel = null;
            }

            if (BadgeLabel != null) {
                BadgeLabel.Dispose ();
                BadgeLabel = null;
            }

            if (BadgeView != null) {
                BadgeView.Dispose ();
                BadgeView = null;
            }

            if (ContentLabel != null) {
                ContentLabel.Dispose ();
                ContentLabel = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }
        }
    }
}
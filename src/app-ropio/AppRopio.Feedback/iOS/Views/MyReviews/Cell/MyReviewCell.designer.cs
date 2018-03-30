// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.MyReviews.Cell
{
    [Register ("MyReviewCell")]
    partial class MyReviewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel BadgeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BadgeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel DateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProductImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel ProductTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel ReviewTextLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BadgeLabel != null) {
                BadgeLabel.Dispose ();
                BadgeLabel = null;
            }

            if (BadgeView != null) {
                BadgeView.Dispose ();
                BadgeView = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }

            if (ProductImageView != null) {
                ProductImageView.Dispose ();
                ProductImageView = null;
            }

            if (ProductTitleLabel != null) {
                ProductTitleLabel.Dispose ();
                ProductTitleLabel = null;
            }

            if (ReviewTextLabel != null) {
                ReviewTextLabel.Dispose ();
                ReviewTextLabel = null;
            }
        }
    }
}
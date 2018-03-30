// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Score
{
    [Register ("ScoreCell")]
    partial class ScoreCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundHighlightView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BackgroundHighlightWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ScoreView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundHighlightView != null) {
                BackgroundHighlightView.Dispose ();
                BackgroundHighlightView = null;
            }

            if (BackgroundHighlightWidthConstraint != null) {
                BackgroundHighlightWidthConstraint.Dispose ();
                BackgroundHighlightWidthConstraint = null;
            }

            if (BackgroundView != null) {
                BackgroundView.Dispose ();
                BackgroundView = null;
            }

            if (ScoreView != null) {
                ScoreView.Dispose ();
                ScoreView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
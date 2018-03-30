// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Feedback.iOS.Views.ReviewDetails
{
    [Register ("ScoreView")]
    partial class ScoreView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView OverlayView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel ScoreLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundView != null) {
                BackgroundView.Dispose ();
                BackgroundView = null;
            }

            if (OverlayView != null) {
                OverlayView.Dispose ();
                OverlayView = null;
            }

            if (ScoreLabel != null) {
                ScoreLabel.Dispose ();
                ScoreLabel = null;
            }
        }
    }
}
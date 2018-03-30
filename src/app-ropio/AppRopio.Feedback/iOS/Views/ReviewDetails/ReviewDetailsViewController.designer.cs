// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewDetails
{
    [Register ("ReviewDetailsViewController")]
    partial class ReviewDetailsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ButtonsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel DateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DeleteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EditButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ProductDetailsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ProductImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel ProductTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ScoreContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AppRopio.Base.iOS.Controls.ARLabel TextLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ButtonsView != null) {
                ButtonsView.Dispose ();
                ButtonsView = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }

            if (DeleteButton != null) {
                DeleteButton.Dispose ();
                DeleteButton = null;
            }

            if (EditButton != null) {
                EditButton.Dispose ();
                EditButton = null;
            }

            if (ProductDetailsView != null) {
                ProductDetailsView.Dispose ();
                ProductDetailsView = null;
            }

            if (ProductImageView != null) {
                ProductImageView.Dispose ();
                ProductImageView = null;
            }

            if (ProductTitleLabel != null) {
                ProductTitleLabel.Dispose ();
                ProductTitleLabel = null;
            }

            if (ScoreContainerView != null) {
                ScoreContainerView.Dispose ();
                ScoreContainerView = null;
            }

            if (TextLabel != null) {
                TextLabel.Dispose ();
                TextLabel = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Settings.iOS.Views.Regions.Cell
{
    [Register ("RegionCell")]
    partial class RegionCell
    {
        [Outlet]
        UIKit.UIImageView SelectionImageView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SelectionImageView != null) {
                SelectionImageView.Dispose ();
                SelectionImageView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
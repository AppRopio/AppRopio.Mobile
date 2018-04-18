// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.Base.Settings.iOS.Views.Languages.Cells
{
    [Register("LanguageCell")]
    partial class LanguageCell
    {
        [Outlet]
        UIKit.UIImageView SelectionImageView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (SelectionImageView != null)
            {
                SelectionImageView.Dispose();
                SelectionImageView = null;
            }

            if (TitleLabel != null)
            {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}

// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Settings.iOS.Views.Cells.Picker
{
    [Register ("SettingsPickerCell")]
    partial class SettingsPickerCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel ValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (ValueLabel != null) {
                ValueLabel.Dispose ();
                ValueLabel = null;
            }
        }
    }
}
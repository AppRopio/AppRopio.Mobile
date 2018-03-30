// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Settings.iOS.Views.Cells.Switch
{
    [Register ("SettingsSwitchCell")]
    partial class SettingsSwitchCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }


        [Outlet]
        UIKit.UISwitch ValueSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (ValueSwitch != null) {
                ValueSwitch.Dispose ();
                ValueSwitch = null;
            }
        }
    }
}
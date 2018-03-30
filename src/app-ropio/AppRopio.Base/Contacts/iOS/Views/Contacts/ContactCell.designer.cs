// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Contacts.iOS.Views.Contacts
{
    [Register ("ContactCell")]
    partial class ContactCell
    {
        [Outlet]
        UIKit.UIImageView IconImage { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (IconImage != null) {
                IconImage.Dispose ();
                IconImage = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}
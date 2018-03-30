// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.Cells
{
    [Register ("CCategoryCell")]
    partial class CCategoryCell
    {
        [Outlet]
        UIKit.UIView _bottomIndicator { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }
        }
    }
}
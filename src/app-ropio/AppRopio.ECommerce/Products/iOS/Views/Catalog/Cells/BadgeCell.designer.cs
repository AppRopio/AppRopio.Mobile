// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells
{
    [Register ("BadgeCell")]
    partial class BadgeCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _badge { get; set; }


        [Outlet]
        UIKit.UIView _badgeView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_badge != null) {
                _badge.Dispose ();
                _badge = null;
            }

            if (_badgeView != null) {
                _badgeView.Dispose ();
                _badgeView = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops.Cells
{
    [Register ("PDHShopIndicatorCell")]
    partial class PDHShopIndicatorCell
    {
        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _address { get; set; }


        [Outlet]
        UIKit.UIView _indicator { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_address != null) {
                _address.Dispose ();
                _address = null;
            }

            if (_indicator != null) {
                _indicator.Dispose ();
                _indicator = null;
            }

            if (_name != null) {
                _name.Dispose ();
                _name = null;
            }
        }
    }
}
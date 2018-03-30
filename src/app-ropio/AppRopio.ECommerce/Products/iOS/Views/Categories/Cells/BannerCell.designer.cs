// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cells
{
    [Register ("BannerCell")]
    partial class BannerCell
    {
        [Outlet]
        UIKit.UIImageView _image { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_image != null) {
                _image.Dispose ();
                _image = null;
            }
        }
    }
}
// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductTextContent
{
    [Register ("ProductTextContentViewController")]
    partial class ProductTextContentViewController
    {
        [Outlet]
        UIKit.UITextView _textView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_textView != null) {
                _textView.Dispose ();
                _textView = null;
            }
        }
    }
}
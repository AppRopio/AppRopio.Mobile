// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductWebContent
{
    [Register ("ProductWebContentViewController")]
    partial class ProductWebContentViewController
    {
        [Outlet]
        AppRopio.Base.iOS.BindableWebView _webView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_webView != null) {
                _webView.Dispose ();
                _webView = null;
            }
        }
    }
}
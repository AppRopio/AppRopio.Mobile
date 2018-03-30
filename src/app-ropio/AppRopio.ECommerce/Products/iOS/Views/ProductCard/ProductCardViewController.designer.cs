// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard
{
    [Register ("ProductCardViewController")]
    partial class ProductCardViewController
    {
        [Outlet]
        UIKit.UITableView _tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (_tableView != null) {
                _tableView.Dispose ();
                _tableView = null;
            }
        }
    }
}
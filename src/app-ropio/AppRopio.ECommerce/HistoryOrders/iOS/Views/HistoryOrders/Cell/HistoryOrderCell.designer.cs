// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    [Register ("HistoryOrderCell")]
    partial class HistoryOrderCell
    {
        [Outlet]
        UIKit.UIImageView ArrowImageView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel ItemsLabel { get; set; }


        [Outlet]
        UIKit.UIView ItemsView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel NumberLabel { get; set; }


        [Outlet]
        UIKit.UIImageView OrderImageView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel OrderStatusLabel { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel PaymentStatusLabel { get; set; }


        [Outlet]
        UIKit.UIView PaymentStatusView { get; set; }


        [Outlet]
        AppRopio.Base.iOS.Controls.ARLabel PriceLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ArrowImageView != null) {
                ArrowImageView.Dispose ();
                ArrowImageView = null;
            }

            if (ItemsLabel != null) {
                ItemsLabel.Dispose ();
                ItemsLabel = null;
            }

            if (ItemsView != null) {
                ItemsView.Dispose ();
                ItemsView = null;
            }

            if (NumberLabel != null) {
                NumberLabel.Dispose ();
                NumberLabel = null;
            }

            if (OrderImageView != null) {
                OrderImageView.Dispose ();
                OrderImageView = null;
            }

            if (OrderStatusLabel != null) {
                OrderStatusLabel.Dispose ();
                OrderStatusLabel = null;
            }

            if (PaymentStatusLabel != null) {
                PaymentStatusLabel.Dispose ();
                PaymentStatusLabel = null;
            }

            if (PaymentStatusView != null) {
                PaymentStatusView.Dispose ();
                PaymentStatusView = null;
            }

            if (PriceLabel != null) {
                PriceLabel.Dispose ();
                PriceLabel = null;
            }
        }
    }
}
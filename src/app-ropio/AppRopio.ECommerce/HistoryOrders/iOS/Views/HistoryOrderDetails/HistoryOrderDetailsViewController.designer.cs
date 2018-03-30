// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
	[Register ("HistoryOrderDetailsViewController")]
	partial class HistoryOrderDetailsViewController
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel AmountHintLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UIView ContentView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel DeliveryNameLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel DeliveryPointAddressLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel DeliveryPointNameLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel DeliveryPriceLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel ItemsLabel { get; set; }

		[Outlet]
		UIKit.UIView ItemsView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint OrderStatusHeight { get; set; }

		[Outlet]
		UIKit.UITableView OrderStatusTableView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel PaymentHintLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel PaymentNameLabel { get; set; }

		[Outlet]
		UIKit.UIButton RepeatButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PaymentHintLabel != null) {
				PaymentHintLabel.Dispose ();
				PaymentHintLabel = null;
			}

			if (AmountHintLabel != null) {
				AmountHintLabel.Dispose ();
				AmountHintLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}

			if (ContentView != null) {
				ContentView.Dispose ();
				ContentView = null;
			}

			if (DeliveryNameLabel != null) {
				DeliveryNameLabel.Dispose ();
				DeliveryNameLabel = null;
			}

			if (DeliveryPointAddressLabel != null) {
				DeliveryPointAddressLabel.Dispose ();
				DeliveryPointAddressLabel = null;
			}

			if (DeliveryPointNameLabel != null) {
				DeliveryPointNameLabel.Dispose ();
				DeliveryPointNameLabel = null;
			}

			if (DeliveryPriceLabel != null) {
				DeliveryPriceLabel.Dispose ();
				DeliveryPriceLabel = null;
			}

			if (ItemsLabel != null) {
				ItemsLabel.Dispose ();
				ItemsLabel = null;
			}

			if (ItemsView != null) {
				ItemsView.Dispose ();
				ItemsView = null;
			}

			if (OrderStatusHeight != null) {
				OrderStatusHeight.Dispose ();
				OrderStatusHeight = null;
			}

			if (OrderStatusTableView != null) {
				OrderStatusTableView.Dispose ();
				OrderStatusTableView = null;
			}

			if (PaymentNameLabel != null) {
				PaymentNameLabel.Dispose ();
				PaymentNameLabel = null;
			}

			if (RepeatButton != null) {
				RepeatButton.Dispose ();
				RepeatButton = null;
			}
		}
	}
}

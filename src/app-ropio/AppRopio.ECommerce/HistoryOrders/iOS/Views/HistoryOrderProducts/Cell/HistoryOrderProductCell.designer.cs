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
	[Register ("HistoryOrderProductCell")]
	partial class HistoryOrderProductCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel AmountLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel BadgeLabel { get; set; }

		[Outlet]
		UIKit.UIView BadgeView { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView ItemImageView { get; set; }

		[Outlet]
		UIKit.UIView OverlayView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel TitleLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel TotalPriceLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (OverlayView != null) {
				OverlayView.Dispose ();
				OverlayView = null;
			}

			if (BadgeView != null) {
				BadgeView.Dispose ();
				BadgeView = null;
			}

			if (BadgeLabel != null) {
				BadgeLabel.Dispose ();
				BadgeLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}

			if (ItemImageView != null) {
				ItemImageView.Dispose ();
				ItemImageView = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (TotalPriceLabel != null) {
				TotalPriceLabel.Dispose ();
				TotalPriceLabel = null;
			}
		}
	}
}

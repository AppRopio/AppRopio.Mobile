// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Full
{
	[Register ("FullOrderViewController")]
	partial class FullOrderViewController
	{
		[Outlet]
		UIKit.UIView _bottomView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _deliveryPriceLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _deliveryPriceTitle { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView _deliveryTimeActivityIndicator { get; set; }

		[Outlet]
		UIKit.UIImageView _deliveryTimeIconView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _deliveryTimeLabel { get; set; }

		[Outlet]
		UIKit.UIView _deliveryTimeSeparatorView { get; set; }

		[Outlet]
		UIKit.UIView _deliveryTimeView { get; set; }

		[Outlet]
		UIKit.UIButton _nextButton { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_bottomView != null) {
				_bottomView.Dispose ();
				_bottomView = null;
			}

			if (_deliveryPriceLabel != null) {
				_deliveryPriceLabel.Dispose ();
				_deliveryPriceLabel = null;
			}

			if (_deliveryPriceTitle != null) {
				_deliveryPriceTitle.Dispose ();
				_deliveryPriceTitle = null;
			}

			if (_deliveryTimeActivityIndicator != null) {
				_deliveryTimeActivityIndicator.Dispose ();
				_deliveryTimeActivityIndicator = null;
			}

			if (_deliveryTimeIconView != null) {
				_deliveryTimeIconView.Dispose ();
				_deliveryTimeIconView = null;
			}

			if (_deliveryTimeLabel != null) {
				_deliveryTimeLabel.Dispose ();
				_deliveryTimeLabel = null;
			}

			if (_deliveryTimeSeparatorView != null) {
				_deliveryTimeSeparatorView.Dispose ();
				_deliveryTimeSeparatorView = null;
			}

			if (_deliveryTimeView != null) {
				_deliveryTimeView.Dispose ();
				_deliveryTimeView = null;
			}

			if (_nextButton != null) {
				_nextButton.Dispose ();
				_nextButton = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}
		}
	}
}

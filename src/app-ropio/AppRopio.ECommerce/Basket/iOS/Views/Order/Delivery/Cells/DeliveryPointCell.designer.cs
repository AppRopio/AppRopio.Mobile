// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Cells
{
	[Register ("DeliveryPointCell")]
	partial class DeliveryPointCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _addressLabel { get; set; }

		[Outlet]
		UIKit.UIStackView _buttonsView { get; set; }

		[Outlet]
		UIKit.UIButton _callButton { get; set; }

		[Outlet]
		UIKit.UIImageView _checkImageView { get; set; }

		[Outlet]
		UIKit.UIImageView _distanceImageView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _distanceLabel { get; set; }

		[Outlet]
		UIKit.UIStackView _distanceView { get; set; }

		[Outlet]
		UIKit.UIButton _infoButton { get; set; }

		[Outlet]
		UIKit.UIButton _mapButton { get; set; }

		[Outlet]
		UIKit.UIButton _routeButton { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_addressLabel != null) {
				_addressLabel.Dispose ();
				_addressLabel = null;
			}

			if (_buttonsView != null) {
				_buttonsView.Dispose ();
				_buttonsView = null;
			}

			if (_callButton != null) {
				_callButton.Dispose ();
				_callButton = null;
			}

			if (_checkImageView != null) {
				_checkImageView.Dispose ();
				_checkImageView = null;
			}

			if (_distanceImageView != null) {
				_distanceImageView.Dispose ();
				_distanceImageView = null;
			}

			if (_distanceLabel != null) {
				_distanceLabel.Dispose ();
				_distanceLabel = null;
			}

			if (_distanceView != null) {
				_distanceView.Dispose ();
				_distanceView = null;
			}

			if (_infoButton != null) {
				_infoButton.Dispose ();
				_infoButton = null;
			}

			if (_mapButton != null) {
				_mapButton.Dispose ();
				_mapButton = null;
			}

			if (_routeButton != null) {
				_routeButton.Dispose ();
				_routeButton = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}
		}
	}
}

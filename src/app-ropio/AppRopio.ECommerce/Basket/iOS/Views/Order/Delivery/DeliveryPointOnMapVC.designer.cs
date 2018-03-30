// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery
{
	[Register ("DeliveryPointOnMapVC")]
	partial class DeliveryPointOnMapVC
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _addressLabel { get; set; }

		[Outlet]
		UIKit.UIButton _callButton { get; set; }

		[Outlet]
		UIKit.UIImageView _distanceImageView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _distanceLabel { get; set; }

		[Outlet]
		UIKit.UIStackView _distanceView { get; set; }

		[Outlet]
		UIKit.UIView _infoBlockView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _infoLabel { get; set; }

		[Outlet]
		UIKit.UIView _infoTopLineView { get; set; }

		[Outlet]
		MapKit.MKMapView _mapView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _mapViewTopConstraint { get; set; }

		[Outlet]
		UIKit.UIButton _routeButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _stackViewBottomConstraint { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _workTimeLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_addressLabel != null) {
				_addressLabel.Dispose ();
				_addressLabel = null;
			}

			if (_callButton != null) {
				_callButton.Dispose ();
				_callButton = null;
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

			if (_infoBlockView != null) {
				_infoBlockView.Dispose ();
				_infoBlockView = null;
			}

			if (_infoLabel != null) {
				_infoLabel.Dispose ();
				_infoLabel = null;
			}

			if (_infoTopLineView != null) {
				_infoTopLineView.Dispose ();
				_infoTopLineView = null;
			}

			if (_mapView != null) {
				_mapView.Dispose ();
				_mapView = null;
			}

			if (_mapViewTopConstraint != null) {
				_mapViewTopConstraint.Dispose ();
				_mapViewTopConstraint = null;
			}

			if (_stackViewBottomConstraint != null) {
				_stackViewBottomConstraint.Dispose ();
				_stackViewBottomConstraint = null;
			}

			if (_routeButton != null) {
				_routeButton.Dispose ();
				_routeButton = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_workTimeLabel != null) {
				_workTimeLabel.Dispose ();
				_workTimeLabel = null;
			}
		}
	}
}

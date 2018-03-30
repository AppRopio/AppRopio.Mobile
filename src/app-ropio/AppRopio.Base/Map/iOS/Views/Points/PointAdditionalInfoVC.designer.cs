// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Map.iOS.Views.Points
{
	[Register ("PointAdditionalInfoVC")]
	partial class PointAdditionalInfoVC
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _addressLabel { get; set; }

		[Outlet]
		UIKit.UIImageView _distanceImageView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _distanceLabel { get; set; }

		[Outlet]
		UIKit.UIStackView _distanceView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _infoLabel { get; set; }

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

			if (_infoLabel != null) {
				_infoLabel.Dispose ();
				_infoLabel = null;
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

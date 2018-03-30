// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;

namespace AppRopio.Base.Map.iOS.Views.Points.List.Cells
{
    [Register("PointCell")]
    partial class PointCell
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

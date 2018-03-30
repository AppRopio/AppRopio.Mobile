// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Navigation.Menu.iOS.Views
{
	[Register ("MenuCell")]
	partial class MenuCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _badge { get; set; }

		[Outlet]
		UIKit.UIView _badgeView { get; set; }

		[Outlet]
		UIKit.UIImageView _icon { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _stackViewLeftConstraint { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_badge != null) {
				_badge.Dispose ();
				_badge = null;
			}

			if (_badgeView != null) {
				_badgeView.Dispose ();
				_badgeView = null;
			}

			if (_icon != null) {
				_icon.Dispose ();
				_icon = null;
			}

			if (_stackViewLeftConstraint != null) {
				_stackViewLeftConstraint.Dispose ();
				_stackViewLeftConstraint = null;
			}

			if (_title != null) {
				_title.Dispose ();
				_title = null;
			}
		}
	}
}

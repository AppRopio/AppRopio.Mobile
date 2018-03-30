// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
	[Register ("OrderFieldOptionalCell")]
	partial class OrderFieldOptionalCell
	{
		[Outlet]
		UIKit.UIView _bottomSeparatorView { get; set; }

		[Outlet]
		UIKit.UIView _middleSeparatorView { get; set; }

		[Outlet]
		UIKit.UISwitch _switch { get; set; }

		[Outlet]
		UIKit.UITextView _textView { get; set; }

		[Outlet]
		UIKit.UIView _textViewLayout { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }

		[Outlet]
		UIKit.UIView _titleLayout { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_switch != null) {
				_switch.Dispose ();
				_switch = null;
			}

			if (_titleLayout != null) {
				_titleLayout.Dispose ();
				_titleLayout = null;
			}

			if (_textViewLayout != null) {
				_textViewLayout.Dispose ();
				_textViewLayout = null;
			}

			if (_textView != null) {
				_textView.Dispose ();
				_textView = null;
			}

			if (_middleSeparatorView != null) {
				_middleSeparatorView.Dispose ();
				_middleSeparatorView = null;
			}

			if (_bottomSeparatorView != null) {
				_bottomSeparatorView.Dispose ();
				_bottomSeparatorView = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
	[Register ("OrderFieldCell")]
	partial class OrderFieldCell
	{
		[Outlet]
		UIKit.UIButton _actionButton { get; set; }

		[Outlet]
		UIKit.UIImageView _iconImageView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARTextField _textField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_iconImageView != null) {
				_iconImageView.Dispose ();
				_iconImageView = null;
			}

			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}

			if (_actionButton != null) {
				_actionButton.Dispose ();
				_actionButton = null;
			}
		}
	}
}

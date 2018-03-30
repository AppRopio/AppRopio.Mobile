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
	[Register ("OrderFieldCounterCell")]
	partial class OrderFieldCounterCell
	{
		[Outlet]
		UIKit.UIView _bottomSeparator { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _topViewHeightConstraint { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _valueName { get; set; }

		[Outlet]
		UIKit.UIPickerView _valuePicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_topViewHeightConstraint != null) {
				_topViewHeightConstraint.Dispose ();
				_topViewHeightConstraint = null;
			}

			if (_valuePicker != null) {
				_valuePicker.Dispose ();
				_valuePicker = null;
			}

			if (_bottomSeparator != null) {
				_bottomSeparator.Dispose ();
				_bottomSeparator = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_valueName != null) {
				_valueName.Dispose ();
				_valueName = null;
			}
		}
	}
}

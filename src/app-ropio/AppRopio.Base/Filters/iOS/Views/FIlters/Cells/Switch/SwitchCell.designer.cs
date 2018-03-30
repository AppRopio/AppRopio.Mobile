// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Switch
{
	[Register ("SwitchCell")]
	partial class SwitchCell
	{
		[Outlet]
		UIKit.UIView _bottomSeparator { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		UIKit.UISwitch _switch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_bottomSeparator != null) {
				_bottomSeparator.Dispose ();
				_bottomSeparator = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_switch != null) {
				_switch.Dispose ();
				_switch = null;
			}
		}
	}
}

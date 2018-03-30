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
	[Register ("SectionHeader")]
	partial class SectionHeader
	{
		[Outlet]
		UIKit.UIView _bottomSeparatorView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_bottomSeparatorView != null) {
				_bottomSeparatorView.Dispose ();
				_bottomSeparatorView = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}
		}
	}
}

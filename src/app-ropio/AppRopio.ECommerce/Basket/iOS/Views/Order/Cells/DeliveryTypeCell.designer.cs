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
	[Register ("DeliveryTypeCell")]
	partial class DeliveryTypeCell
	{
		[Outlet]
		UIKit.UIImageView _checkImageView { get; set; }

		[Outlet]
		UIKit.UIImageView _disclosureImageView { get; set; }

		[Outlet]
		UIKit.UIView _separatorView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_checkImageView != null) {
				_checkImageView.Dispose ();
				_checkImageView = null;
			}

			if (_disclosureImageView != null) {
				_disclosureImageView.Dispose ();
				_disclosureImageView = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}

			if (_separatorView != null) {
				_separatorView.Dispose ();
				_separatorView = null;
			}
		}
	}
}

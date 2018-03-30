// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Basket.Cells
{
	[Register ("BasketCell")]
	partial class BasketCell
	{
		[Outlet]
		UIKit.UIButton _decButton { get; set; }

		[Outlet]
		UIKit.UIImageView _imageView { get; set; }

		[Outlet]
		UIKit.UIButton _incButton { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _priceLabel { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView _quantityActivityIndicator { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _quantityLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _titleLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_decButton != null) {
				_decButton.Dispose ();
				_decButton = null;
			}

			if (_imageView != null) {
				_imageView.Dispose ();
				_imageView = null;
			}

			if (_incButton != null) {
				_incButton.Dispose ();
				_incButton = null;
			}

			if (_priceLabel != null) {
				_priceLabel.Dispose ();
				_priceLabel = null;
			}

			if (_quantityActivityIndicator != null) {
				_quantityActivityIndicator.Dispose ();
				_quantityActivityIndicator = null;
			}

			if (_quantityLabel != null) {
				_quantityLabel.Dispose ();
				_quantityLabel = null;
			}

			if (_titleLabel != null) {
				_titleLabel.Dispose ();
				_titleLabel = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.ShortInfo
{
	[Register ("ModalShortInfoCell")]
	partial class ModalShortInfoCell
	{
		[Outlet]
		UIKit.UICollectionView _badges { get; set; }

		[Outlet]
		UIKit.UIView _bottomSeparator { get; set; }

		[Outlet]
		UIKit.UILabel _extraPrice { get; set; }

		[Outlet]
		UIKit.UIButton _markButton { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _oldPrice { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _price { get; set; }

		[Outlet]
		UIKit.UIButton _shareButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_extraPrice != null) {
				_extraPrice.Dispose ();
				_extraPrice = null;
			}

			if (_badges != null) {
				_badges.Dispose ();
				_badges = null;
			}

			if (_bottomSeparator != null) {
				_bottomSeparator.Dispose ();
				_bottomSeparator = null;
			}

			if (_markButton != null) {
				_markButton.Dispose ();
				_markButton = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_oldPrice != null) {
				_oldPrice.Dispose ();
				_oldPrice = null;
			}

			if (_price != null) {
				_price.Dispose ();
				_price = null;
			}

			if (_shareButton != null) {
				_shareButton.Dispose ();
				_shareButton = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.ShortInfo
{
	[Register ("ShortInfoCell")]
	partial class ShortInfoCell
	{
		[Outlet]
		UIKit.UICollectionView _badges { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _badgesWidthContraint { get; set; }

		[Outlet]
		UIKit.UIView _bottomSeparator { get; set; }

		[Outlet]
		UIKit.UILabel _extraPrice { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _fromHintLabel { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _oldPrice { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _price { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _status { get; set; }
		
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

			if (_badgesWidthContraint != null) {
				_badgesWidthContraint.Dispose ();
				_badgesWidthContraint = null;
			}

			if (_bottomSeparator != null) {
				_bottomSeparator.Dispose ();
				_bottomSeparator = null;
			}

			if (_fromHintLabel != null) {
				_fromHintLabel.Dispose ();
				_fromHintLabel = null;
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

			if (_status != null) {
				_status.Dispose ();
				_status = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells
{
	[Register ("CatalogGridCell")]
	partial class CatalogGridCell
	{
		[Outlet]
		UIKit.UIButton _actionButton { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _actionButtonBottomMarginConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _actionButtonHeightConstraint { get; set; }

		[Outlet]
		UIKit.UICollectionView _badges { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _badgesWidthContraint { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView _image { get; set; }

		[Outlet]
		UIKit.UIButton _markButton { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _maxPrice { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _oldPrice { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _price { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_actionButton != null) {
				_actionButton.Dispose ();
				_actionButton = null;
			}

			if (_badges != null) {
				_badges.Dispose ();
				_badges = null;
			}

			if (_badgesWidthContraint != null) {
				_badgesWidthContraint.Dispose ();
				_badgesWidthContraint = null;
			}

			if (_image != null) {
				_image.Dispose ();
				_image = null;
			}

			if (_markButton != null) {
				_markButton.Dispose ();
				_markButton = null;
			}

			if (_maxPrice != null) {
				_maxPrice.Dispose ();
				_maxPrice = null;
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

			if (_actionButtonHeightConstraint != null) {
				_actionButtonHeightConstraint.Dispose ();
				_actionButtonHeightConstraint = null;
			}

			if (_actionButtonBottomMarginConstraint != null) {
				_actionButtonBottomMarginConstraint.Dispose ();
				_actionButtonBottomMarginConstraint = null;
			}
		}
	}
}

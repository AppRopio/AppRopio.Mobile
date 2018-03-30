// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical
{
	[Register ("VerticalCollectionCell")]
	partial class VerticalCollectionCell
	{
		[Outlet]
		UIKit.NSLayoutConstraint _bottomCollectionViewConstraint { get; set; }

		[Outlet]
		UIKit.UIView _bottomSeparator { get; set; }

		[Outlet]
		UIKit.UICollectionView _collectionView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_bottomSeparator != null) {
				_bottomSeparator.Dispose ();
				_bottomSeparator = null;
			}

			if (_collectionView != null) {
				_collectionView.Dispose ();
				_collectionView = null;
			}

			if (_bottomCollectionViewConstraint != null) {
				_bottomCollectionViewConstraint.Dispose ();
				_bottomCollectionViewConstraint = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}
		}
	}
}

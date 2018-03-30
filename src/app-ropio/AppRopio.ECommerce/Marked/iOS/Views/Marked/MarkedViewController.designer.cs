// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Marked.iOS.Views.Marked
{
	[Register ("MarkedViewController")]
	partial class MarkedViewController
	{
		[Outlet]
		UIKit.UIButton _basketButton { get; set; }

		[Outlet]
		UIKit.UICollectionView _collectionView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _collectionViewBottomConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _collectionViewTopConstraint { get; set; }

		[Outlet]
		UIKit.UIImageView _emptyImage { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _emptyText { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _emptyTitle { get; set; }

		[Outlet]
		UIKit.UIView _emptyView { get; set; }

		[Outlet]
		UIKit.UIButton _goToButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_goToButton != null) {
				_goToButton.Dispose ();
				_goToButton = null;
			}

			if (_basketButton != null) {
				_basketButton.Dispose ();
				_basketButton = null;
			}

			if (_collectionView != null) {
				_collectionView.Dispose ();
				_collectionView = null;
			}

			if (_collectionViewBottomConstraint != null) {
				_collectionViewBottomConstraint.Dispose ();
				_collectionViewBottomConstraint = null;
			}

			if (_collectionViewTopConstraint != null) {
				_collectionViewTopConstraint.Dispose ();
				_collectionViewTopConstraint = null;
			}

			if (_emptyImage != null) {
				_emptyImage.Dispose ();
				_emptyImage = null;
			}

			if (_emptyText != null) {
				_emptyText.Dispose ();
				_emptyText = null;
			}

			if (_emptyTitle != null) {
				_emptyTitle.Dispose ();
				_emptyTitle = null;
			}

			if (_emptyView != null) {
				_emptyView.Dispose ();
				_emptyView = null;
			}
		}
	}
}

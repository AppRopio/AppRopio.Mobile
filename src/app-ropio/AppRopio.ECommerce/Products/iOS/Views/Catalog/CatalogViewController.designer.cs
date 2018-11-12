// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog
{
	[Register ("CatalogViewController")]
	partial class CatalogViewController
	{
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
		UIKit.UIView _emptyView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _epmtyTitle { get; set; }

		[Outlet]
		UIKit.UIButton _goToButton { get; set; }

		[Outlet]
		UIKit.UISearchBar _searchBar { get; set; }

		[Outlet]
		UIKit.UIButton _searchButton { get; set; }

		[Outlet]
		UIKit.UIView _searchView { get; set; }

		[Outlet]
		UIKit.UIStackView _stackView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_collectionView != null) {
				_collectionView.Dispose ();
				_collectionView = null;
			}

			if (_emptyImage != null) {
				_emptyImage.Dispose ();
				_emptyImage = null;
			}

			if (_emptyText != null) {
				_emptyText.Dispose ();
				_emptyText = null;
			}

			if (_emptyView != null) {
				_emptyView.Dispose ();
				_emptyView = null;
			}

			if (_epmtyTitle != null) {
				_epmtyTitle.Dispose ();
				_epmtyTitle = null;
			}

			if (_goToButton != null) {
				_goToButton.Dispose ();
				_goToButton = null;
			}

			if (_searchBar != null) {
				_searchBar.Dispose ();
				_searchBar = null;
			}

			if (_searchButton != null) {
				_searchButton.Dispose ();
				_searchButton = null;
			}

			if (_searchView != null) {
				_searchView.Dispose ();
				_searchView = null;
			}

			if (_stackView != null) {
				_stackView.Dispose ();
				_stackView = null;
			}

			if (_collectionViewTopConstraint != null) {
				_collectionViewTopConstraint.Dispose ();
				_collectionViewTopConstraint = null;
			}

			if (_collectionViewBottomConstraint != null) {
				_collectionViewBottomConstraint.Dispose ();
				_collectionViewBottomConstraint = null;
			}
		}
	}
}

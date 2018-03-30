// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Sort
{
	[Register ("SortViewController")]
	partial class SortViewController
	{
		[Outlet]
		UIKit.UIButton _cancelBtn { get; set; }

		[Outlet]
		UIKit.UIView _containerView { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _containerViewHeightConstraint { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint _containerViewTopConstraint { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _title { get; set; }

		[Outlet]
		UIKit.UIView _titleSeparator { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_titleSeparator != null) {
				_titleSeparator.Dispose ();
				_titleSeparator = null;
			}

			if (_cancelBtn != null) {
				_cancelBtn.Dispose ();
				_cancelBtn = null;
			}

			if (_containerView != null) {
				_containerView.Dispose ();
				_containerView = null;
			}

			if (_containerViewHeightConstraint != null) {
				_containerViewHeightConstraint.Dispose ();
				_containerViewHeightConstraint = null;
			}

			if (_containerViewTopConstraint != null) {
				_containerViewTopConstraint.Dispose ();
				_containerViewTopConstraint = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}

			if (_title != null) {
				_title.Dispose ();
				_title = null;
			}
		}
	}
}

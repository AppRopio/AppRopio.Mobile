// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Filters
{
	[Register ("FiltersViewController")]
	partial class FiltersViewController
	{
		[Outlet]
		UIKit.UIButton _applyBtn { get; set; }

		[Outlet]
		UIKit.UITableView _tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_applyBtn != null) {
				_applyBtn.Dispose ();
				_applyBtn = null;
			}

			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}
		}
	}
}

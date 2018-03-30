// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Map.iOS.Views.Points.List
{
	[Register ("PointsListViewController")]
	partial class PointsListViewController
	{
		[Outlet]
		UIKit.UITableView _tableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_tableView != null) {
				_tableView.Dispose ();
				_tableView = null;
			}
		}
	}
}

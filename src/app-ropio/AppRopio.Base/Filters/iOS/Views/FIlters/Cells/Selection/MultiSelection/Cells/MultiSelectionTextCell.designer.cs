// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Selection.MultiSelection.Cells
{
	[Register ("MultiSelectionTextCell")]
	partial class MultiSelectionTextCell
	{
		[Outlet]
		UIKit.UIImageView _crossImageView { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		UIKit.UIView _selectedView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_crossImageView != null) {
				_crossImageView.Dispose ();
				_crossImageView = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_selectedView != null) {
				_selectedView.Dispose ();
				_selectedView = null;
			}
		}
	}
}

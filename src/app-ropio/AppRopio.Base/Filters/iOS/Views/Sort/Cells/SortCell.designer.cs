// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Filters.iOS.Views.Sort.Cells
{
	[Register ("SortCell")]
	partial class SortCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		UIKit.UIImageView _selectionImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_selectionImageView != null) {
				_selectionImageView.Dispose ();
				_selectionImageView = null;
			}
		}
	}
}

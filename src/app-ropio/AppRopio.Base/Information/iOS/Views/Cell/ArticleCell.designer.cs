// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Information.iOS.Views.Cell
{
	[Register ("ArticleCell")]
	partial class ArticleCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_title != null) {
				_title.Dispose ();
				_title = null;
			}
		}
	}
}

// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio_Test.Views
{
	[Register ("DemoViewController")]
	partial class DemoViewController
	{
		[Outlet]
		UIKit.UIButton _agreementButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_agreementButton != null) {
				_agreementButton.Dispose ();
				_agreementButton = null;
			}
		}
	}
}

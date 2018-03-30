// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.Base.Auth.iOS.Views.SignUp.Cells
{
	[Register ("SignUpItemBaseViewCell")]
	partial class SignUpItemBaseViewCell
	{
		[Outlet]
		AppRopio.Base.iOS.Controls.ARTextField _textField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_textField != null) {
				_textField.Dispose ();
				_textField = null;
			}
		}
	}
}

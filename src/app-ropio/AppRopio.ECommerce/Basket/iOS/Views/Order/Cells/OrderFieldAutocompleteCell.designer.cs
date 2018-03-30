// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
	[Register ("OrderFieldAutocompleteCell")]
	partial class OrderFieldAutocompleteCell
	{
		[Outlet]
		UIKit.UILabel _valueLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_valueLabel != null) {
				_valueLabel.Dispose ();
				_valueLabel = null;
			}
		}
	}
}

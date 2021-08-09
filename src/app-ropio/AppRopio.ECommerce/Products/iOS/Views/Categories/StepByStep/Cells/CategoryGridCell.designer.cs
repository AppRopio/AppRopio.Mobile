// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep.Cells
{
	[Register ("CategoryGridCell")]
	partial class CategoryGridCell
	{
		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView _backgroundImage { get; set; }

		[Outlet]
		UIKit.UIView _gradientView { get; set; }

		[Outlet]
		FFImageLoading.Cross.MvxCachedImageView _image { get; set; }

		[Outlet]
		AppRopio.Base.iOS.Controls.ARLabel _name { get; set; }

		[Outlet]
		UIKit.UIView _separator { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (_gradientView != null) {
				_gradientView.Dispose ();
				_gradientView = null;
			}

			if (_backgroundImage != null) {
				_backgroundImage.Dispose ();
				_backgroundImage = null;
			}

			if (_image != null) {
				_image.Dispose ();
				_image = null;
			}

			if (_name != null) {
				_name.Dispose ();
				_name = null;
			}

			if (_separator != null) {
				_separator.Dispose ();
				_separator = null;
			}
		}
	}
}
